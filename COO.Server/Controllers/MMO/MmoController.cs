using System.Net.Http;
using COO.Business.Logic.MMO.Write.CreateCharacter;
using COO.Server.Controllers.MMO.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.Business.Logic.MMO.Read.GetAllianceClans;
using COO.Business.Logic.MMO.Read.GetAlliances;
using COO.Business.Logic.MMO.Read.GetCharacter;
using COO.Business.Logic.MMO.Read.GetCharacters;
using COO.Business.Logic.MMO.Read.GetClanCharacters;
using COO.Business.Logic.MMO.Read.GetClans;
using COO.Business.Logic.MMO.Write.DeleteCharacter;
using COO.Business.Logic.MMO.Read.GetGameServers;
using COO.Business.Logic.MMO.Write.AddCharacterToClan;
using COO.Business.Logic.MMO.Write.AddClanToAlliance;
using COO.Business.Logic.MMO.Write.CreateAlliance;
using COO.Business.Logic.MMO.Write.CreateClan;
using COO.Business.Logic.MMO.Write.DeleteCharacterFromClan;
using COO.Business.Logic.MMO.Write.DeleteClanFromAlliance;
using COO.Business.Logic.MMO.Write.DisbandAlliance;
using COO.Business.Logic.MMO.Write.LeaveFromClan;
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
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetGameServersQuery()));
        }

        [HttpPost]
        [Route(nameof(CreateCharacter))]
        public async Task<ActionResult> CreateCharacter(CreateCharacterRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new CreateCharacterCommand(UserId(), model.Name, model.Gender, model.RaceId, model.ClassId, model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(DeleteCharacter))]
        public async Task<ActionResult> DeleteCharacter(DeleteCharacterRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new DeleteCharacterCommand(model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(GetCharacter))]
        public async Task<ActionResult> GetCharacter(GetCharacterRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId(), model.CharacterId));
            return Ok(await _mediator.Send(new GetCharacterQuery(model.CharacterId, model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(GetCharacters))]
        public async Task<ActionResult> GetCharacters(GetCharactersRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetCharactersQuery(UserId(), model.ServerId)));
        }

        [HttpPost]
        [Route(nameof(UpdateCharacter))]
        public async Task<ActionResult> UpdateCharacter(UpdateCharacterRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(
                new UpdateCharacterCommand(
                    model.CharacterId, model.ClassId, model.Health, model.Mana,
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
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new CreateClanCommand(model.CharacterId, model.ClanName)));
        }

        [HttpPost]
        [Route(nameof(AddCharacterToClan))]
        public async Task<ActionResult> AddCharacterToClan(AddCharacterToClanRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new AddCharacterToClanCommand(model.ClanId, model.CharacterName)));
        }

        [HttpPost]
        [Route(nameof(DisbandClan))]
        public async Task<ActionResult> DisbandClan(DisbandClanRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new DisbandClanCommand(model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(ClanCharacters))]
        public async Task<ActionResult> ClanCharacters(ClanCharactersRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetClanCharactersQuery(model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(LeaveFromClan))]
        public async Task<ActionResult> LeaveFromClan(LeaveFromClanRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new LeaveFromClanCommand(model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(DeleteCharacterFromClan))]
        public async Task<ActionResult> DeleteCharacterFromClan(DeleteCharacterFromClanRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new DeleteCharacterFromClanCommand(model.CharacterId, model.CharacterName)));
        }

        [HttpPost]
        [Route(nameof(Clans))]
        public async Task<ActionResult> Clans()
        {

            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetClansQuery(UserId())));
        }

        [HttpPost]
        [Route(nameof(CreateAlliance))]
        public async Task<ActionResult> CreateAlliance(CreateAllianceRequetModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new CreateAllianceCommand(model.CharacterId, model.AllianceName)));
        }

        [HttpPost]
        [Route(nameof(AddClanToAlliance))]
        public async Task<ActionResult> AddClanToAlliance(AddClanToAllianceRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new AddClanToAllianceCommand(model.CharacterId, model.ClanName)));
        }

        [HttpPost]
        [Route(nameof(DisbandAlliance))]
        public async Task<ActionResult> DisbandAlliance(DisbandAllianceRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new DisbandAllianceCommand(model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(DeleteClanFromAlliance))]
        public async Task<ActionResult> DeleteClanFromAlliance(DeleteClanFromAllianceRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new DeleteClanFromAllianceCommand(model.CharacterId, model.ClanName)));
        }

        [HttpPost]
        [Route(nameof(AllianceClans))]
        public async Task<ActionResult> AllianceClans(AllianceClansRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetAllianceClansQuery(model.AllianceName)));
        }

        [HttpPost]
        [Route(nameof(Alliances))]
        public async Task<ActionResult> Alliances(DeleteClanFromAllianceRequestModel model)
        {
            await _mediator.Send(new UpdateActivityCommand(UserId()));
            return Ok(await _mediator.Send(new GetAlliancesQuery()));
        }
    }
}
