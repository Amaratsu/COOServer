using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetCharacters
{
    public class GetCharactersQueryHandler : IRequestHandler<GetCharactersQuery, GetCharactersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public GetCharactersQueryHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<GetCharactersResponseModel> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var response = new GetCharactersResponseModel { Characters = new List<CharacterModel>() };

                var foundCharacters = await context.Characters
                    .Where(c => c.Id == foundUser.Id && c.ServerId == request.ServerId)
                    .ToListAsync(cancellationToken);

                if (foundCharacters.Count > 0)
                {
                    foundCharacters.ForEach(c =>
                    {
                        response.Characters.Add(new CharacterModel
                        {
                            CharacterId = c.Id,
                            Name = c.Name,
                            Gender = c.Gender,
                            RaceId = c.RaceId,
                            ClassId = c.ClassId,
                            Level = c.Level,
                            Experience = c.Experience,
                            Health = c.Health,
                            Mana = c.Mana,
                            EquipChest = c.EquipChest,
                            EquipFeet = c.EquipFeet,
                            EquipHands = c.EquipHands,
                            EquipHead = c.EquipHead,
                            EquipLegs = c.EquipLegs
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
