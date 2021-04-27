using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using COO.Infrastructure.Helpers;
using COO.Infrastructure.Services.DataHash;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.Account.Write.Registration
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, RegistrationResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IDataHashService _dataHashService;

        public RegistrationCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IDataHashService dataHashService)
        {
            _contextFactory = contextFactory;
            _dataHashService = dataHashService;
        }

        public async Task<RegistrationResponseModel> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == request.Login || u.Email == request.Email, cancellationToken);

            if (user != null)
            {
                throw new AppException("Login and email are already taken");
            }

            var salt = _dataHashService.GenerateSalt();
            var newUser = new User
            {
                UserName = request.Login,
                Email = request.Email,
                PaswordSalt = salt,
                PasswordHash = _dataHashService.EncryptData(request.Password, salt),
                CreateDate = DateTime.UtcNow,
                Token = Helpers.RandomString(20)
            };

            await context.Users.AddAsync(newUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new RegistrationResponseModel { 
                UserId = newUser.Id,
                Email = newUser.Email,
                Token = newUser.Token
            };
        }
    }
}
