using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetGameServers
{
    public class GetGameServersQueryHandler : IRequestHandler<GetGameServersQuery, GetGameServersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetGameServersQueryHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetGameServersResponseModel> Handle(GetGameServersQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

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
                        Port = s.Port,
                        Status = s.Status,
                        CurrentCount = s.CurrentCount,
                        MaxCount = s.MaxCount
                    });
                });
            }

            return response;
        }
    }
}
