using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using COO.Infrastructure.Helpers;
using COO.Infrastructure.Services.DataHash;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Read.GetUserByLogin
{
    public sealed class GetUserByLoginQueryHandler : IRequestHandler<GetUserByLoginQuery, User>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;
        private readonly IDataHashService _dataHashService;

        public GetUserByLoginQueryHandler(IDbContextFactory<COODbContext> contextFactory, IDataHashService dataHashService)
        {
            _contextFactory = contextFactory;
            _dataHashService = dataHashService;
        }

        public async Task<User> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(user => user.Name.ToLower() == request.Login.ToLower() || user.Email.ToLower() == request.Login.ToLower());

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

            await context.SaveChangesAsync();

            return user;
        }
    }
}
