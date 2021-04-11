using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetGameServers
{
    public class GetGameServersQueryHandler : IRequestHandler<GetGameServersQuery, GetGameServersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public GetGameServersQueryHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<GetGameServersResponseModel> Handle(GetGameServersQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.
                    FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var response = new GetGameServersResponseModel { GameServers = new List<GameServerModel>() };

                var gameServers = await context.InfoServers
                    .ToListAsync(cancellationToken);

                if (gameServers.Count > 0)
                {
                    gameServers.ForEach(s =>
                    {
                        response.GameServers.Add(new GameServerModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            IP = s.IP,
                            Status = s.Status,
                            CurrentCount = s.CurrentCount,
                            MaxCount = s.MaxCount
                        });
                    });
                }

                await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

                return response;
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
