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

namespace COO.Business.Logic.MMO.Write.CreateRegister
{
    public class CreateRegisterCommandHandler : IRequestHandler<CreateRegisterCommand, User>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;
        private readonly IDataHashService _dataHashService;

        public CreateRegisterCommandHandler(IDbContextFactory<COODbContext> contextFactory, IDataHashService dataHashService)
        {
            _contextFactory = contextFactory;
            _dataHashService = dataHashService;
        }

        public async Task<User> Handle(CreateRegisterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Name == request.Login || user.Email == request.Email);

            if (foundUser != null)
            {
                throw new AppException("Login and email are already taken");
            }

            var salt = _dataHashService.GenerateSalt();
            var user = new User
            {
                Name = request.Login,
                Email = request.Email,
                PaswordSalt = salt,
                PasswordHash = _dataHashService.EncryptData(request.Password, salt),
                CreateDate = DateTime.UtcNow,
                Token = Helpers.RandomString(20)
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
