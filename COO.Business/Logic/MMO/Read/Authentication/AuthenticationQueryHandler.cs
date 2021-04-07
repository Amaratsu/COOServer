using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using COO.Infrastructure.Services.DataHash;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.MMO.Write.UpdateActivity;

namespace COO.Business.Logic.MMO.Read.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationResponseModel>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;
        private readonly IDataHashService _dataHashService;
        private readonly IMediator _mediator;

        public AuthenticationQueryHandler(IDbContextFactory<COODbContext> contextFactory, IDataHashService dataHashService, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _dataHashService = dataHashService;
            _mediator = mediator;
        }

        public async Task<AuthenticationResponseModel> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Login.ToLower(), cancellationToken);

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

            await _mediator.Send(new UpdateActivityCommand(user.Id), cancellationToken);

            return new AuthenticationResponseModel
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
