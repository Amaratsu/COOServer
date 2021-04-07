using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.GetGameServers
{
    public class GetGameServersCommandHandler : IRequestHandler<GetGameServersCommand, GetGameServersResponseModel>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public GetGameServersCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetGameServersResponseModel> Handle(GetGameServersCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.
                    FirstOrDefaultAsync(u => u.Id == request.UserId && u.Token == request.Token, cancellationToken);

            if (foundUser != null)
            {
                var response = new GetGameServersResponseModel { GameServers = new List<GameServer>() };

                var gameServers = await context.InfoServers
                    .ToListAsync(cancellationToken);

                if (gameServers.Count > 0)
                {
                    gameServers.ForEach(s =>
                    {
                        var gameServer = new GameServer
                        {
                            Id = s.Id,
                            Name = s.Name,
                            IP = s.IP,
                            Status = s.Status,
                            CurrentCount = s.CurrentCount,
                            MaxCount = s.MaxCount
                        };
                        response.GameServers.Add(gameServer);
                    });

                    foundUser.LastActivity = DateTime.UtcNow;

                    context.Users.Update(foundUser);

                    await context.SaveChangesAsync(cancellationToken);
                }

                return response;
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
