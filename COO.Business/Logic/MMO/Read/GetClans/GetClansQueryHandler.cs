using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetClans
{
    public class GetClansQueryHandler : IRequestHandler<GetClansQuery, GetClansResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public GetClansQueryHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<GetClansResponseModel> Handle(GetClansQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var response = new GetClansResponseModel
                {
                    Clans = new List<ClanModel>()
                };

                var foundClans = await context.Clans.ToListAsync(cancellationToken);

                if (foundClans.Count > 0)
                {
                    foundClans.ForEach(fc =>
                    {
                        response.Clans.Add(new ClanModel
                        {
                            Name = fc.Name,
                            LeaderName = fc.LeaderName,
                            OnlineCount = fc.CurrentCountCharacters,
                            CurrentCountCharacters = fc.CurrentCountCharacters,
                            MaxCountCharacters = fc.MaxCountCharacters
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
