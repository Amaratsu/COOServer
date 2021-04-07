using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using COO.Infrastructure.Helpers;
using COO.Infrastructure.Services.DataHash;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.Authentication
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationResponseModel>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;
        private readonly IDataHashService _dataHashService;

        public AuthenticationCommandHandler(IDbContextFactory<COODbContext> contextFactory, IDataHashService dataHashService)
        {
            _contextFactory = contextFactory;
            _dataHashService = dataHashService;
        }

        public async Task<AuthenticationResponseModel> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == request.Login.ToLower(), cancellationToken);

            if (user == null)
            {
                throw new AppException("Login or password is incorrect");
            }

            var passwordIsCorrect = _dataHashService.IsCorrectDataHash(request.Password, user.PasswordHash, user.PaswordSalt);

            if (!passwordIsCorrect)
            {
                throw new AppException("Login or password is incorrect");
            }

            if (!user.EmailConfirmed)
            {
                throw new AppException("Email not verified");
            }

            if (user.IsBlocked)
            {
                throw new AppException("The user is blocked");
            }

            user.LastActivity = DateTime.UtcNow;
            user.Token = Helpers.RandomString(10);
            user.CountFailedLogins = 0;

            context.Users.Update(user);

            await context.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponseModel
            {
                UserId = user.Id,
                Token = user.Token
            };
        }
    }
}
