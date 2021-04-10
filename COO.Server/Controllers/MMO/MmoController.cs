using System.Net.Http;
using COO.Business.Logic.MMO.Write.CreateCharacter;
using COO.Server.Controllers.MMO.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using COO.Business.Logic.MMO.Read.GetCharacter;
using COO.Business.Logic.MMO.Read.GetCharacters;
using COO.Business.Logic.MMO.Read.GetClanCharacters;
using COO.Business.Logic.MMO.Write.DeleteCharacter;
using COO.Business.Logic.MMO.Read.GetGameServers;
using COO.Business.Logic.MMO.Write.AddCharacterToClan;
using COO.Business.Logic.MMO.Write.CreateClan;
using COO.Business.Logic.MMO.Write.DeleteCharacterFromClan;
using COO.Business.Logic.MMO.Write.DisbandClan;
using COO.Business.Logic.MMO.Write.UpdateCharacter;

namespace COO.Server.Controllers.MMO
{
    public class MmoController : ApiController
    {
        private readonly IMediator _mediator;

        public MmoController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(nameof(GameServers))]
        public async Task<ActionResult> GameServers()
        {
            return Ok(await _mediator.Send(new GetGameServersQuery(UserId())));
        }

        [HttpPost]
        [Route(nameof(CreateCharacter))]
        public async Task<ActionResult> CreateCharacter(CreateCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new CreateCharacterCommand(UserId(), model.Name, model.Gender, model.RaceId, model.ClassId, model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(DeleteCharacter))]
        public async Task<ActionResult> DeleteCharacter(DeleteCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new DeleteCharacterCommand(UserId(), model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(Character))]
        public async Task<ActionResult> Character(GetCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new GetCharacterQuery(UserId(), model.CharacterId, model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(Characters))]
        public async Task<ActionResult> Characters(GetCharactersRequestModel model)
        {
            return Ok(await _mediator.Send(new GetCharactersQuery(UserId(), model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(UpdateCharacter))]
        public async Task<ActionResult> UpdateCharacter(UpdateCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(
                new UpdateCharacterCommand(
                    UserId(), model.CharacterId, model.ClassId, model.Health, model.Mana,
                    model.Experience, model.Level, model.PosX, model.PosY, model.PosZ,
                    model.RotationYaw, model.EquipChest, model.EquipFeet, model.EquipHands,
                    model.EquipHead, model.EquipLegs, model.Hotbar0, model.Hotbar1, model.Hotbar2,
                    model.Hotbar3, model.Inventory, model.Quests, model.IsOnline
            )));
        }

        [HttpPost]
        [Route(nameof(ServerIp))]
        public async Task<ActionResult> ServerIp()
        {
            var httpClient = new HttpClient();
            var address = await httpClient.GetStringAsync("https://api.ipify.org");
#if DEBUG
            address = "127.0.0.1";
#endif
            return Ok(address);
        }

        [HttpPost]
        [Route(nameof(CreateClan))]
        public async Task<ActionResult> CreateClan(CreateClanRequestModel model)
        {
            return Ok(await _mediator.Send(new CreateClanCommand(UserId(), model.CharacterId, model.ClanName)));
        }

        [HttpPost]
        [Route(nameof(AddCharacterToClan))]
        public async Task<ActionResult> AddCharacterToClan(AddCharacterToClanRequestModel model)
        {
            return Ok(await _mediator.Send(new AddCharacterToClanCommand(UserId(), model.ClanId, model.CharacterName)));
        }

        [HttpPost]
        [Route(nameof(DisbandClan))]
        public async Task<ActionResult> DisbandClan(DisbandClanRequestModel model)
        {
            return Ok(await _mediator.Send(new DisbandClanCommand(UserId(), model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(ClanCharacters))]
        public async Task<ActionResult> ClanCharacters(ClanCharactersRequestModel model)
        {
            return Ok(await _mediator.Send(new GetClanCharactersCommand(UserId(), model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(DeleteCharacterFromClan))]
        public async Task<ActionResult> DeleteCharacterFromClan(DeleteCharacterFromClanRequestModel model)
        {
            return Ok(await _mediator.Send(new DeleteCharacterFromClanCommand(UserId(), model.CharacterId)));
        }
    }
}
