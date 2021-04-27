using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetAlliances
{
    public class GetAlliancesQueryHandler : IRequestHandler<GetAlliancesQuery, GetAlliancesResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetAlliancesQueryHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetAlliancesResponseModel> Handle(GetAlliancesQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var response = new GetAlliancesResponseModel
            {
                Alliances = new List<AllianceModel>()
            };

            var alliances = await context.Alliances.ToListAsync(cancellationToken);
            if (alliances.Count > 0)
            {
                alliances.ForEach(fa =>
                {
                    response.Alliances.Add(new AllianceModel
                    {
                        Name = fa.Name,
                        LeaderName = fa.LeaderName,
                        CurrentCountClans = fa.CurrentCountClans,
                        MaxCountClans = fa.MaxCountClans
                    });
                });
            }

            return response;
        }
    }
}
