using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetAllianceClans
{
    public class GetAllianceClansHandler : IRequestHandler<GetAllianceClansQuery, GetAllianceClansResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetAllianceClansHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetAllianceClansResponseModel> Handle(GetAllianceClansQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var alliance = await context
                .Alliances
                .FirstOrDefaultAsync(a => string.Equals(a.Name, request.AllianceName, StringComparison.CurrentCultureIgnoreCase), cancellationToken);

            if (alliance != null)
            {
                var response = new GetAllianceClansResponseModel
                {
                    AllianceClans = new List<AllianceClanModel>()
                };

                var clans = await context
                    .Clans
                    .Where(c => c.AllianceId == alliance.Id)
                    .ToListAsync(cancellationToken);

                if (clans.Count > 0)
                {
                    clans.ForEach(c =>
                    {
                        response.AllianceClans.Add(new AllianceClanModel
                        {
                            AllianceId = alliance.Id,
                            Name = c.Name,
                            LeaderName = c.LeaderName
                        });
                    });
                }

                return response;
            }
            else
            {
                throw new AppException("The alliance not found.");
            }
        }
    }
}
