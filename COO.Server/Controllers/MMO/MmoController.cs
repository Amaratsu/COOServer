using System.Net.Http;
using COO.Business.Logic.MMO.Write.CreateCharacter;
using COO.Server.Controllers.MMO.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using COO.Business.Logic.MMO.Read.GetCharacter;
using COO.Business.Logic.MMO.Read.GetCharacters;
using COO.Business.Logic.MMO.Write.DeleteCharacter;
using COO.Business.Logic.MMO.Read.GetGameServers;
using COO.Business.Logic.MMO.Write.AddCharacterToClan;
using COO.Business.Logic.MMO.Write.CreateClan;
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
                    model.Hotbar3, model.Inventory, model.Quests
            )));
        }

        [HttpPost]
        [Route(nameof(GetServerIp))]
        public async Task<ActionResult> GetServerIp()
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

        //        [HttpPost]
        //        [Route(nameof(DisbandClan))]
        //        public async Task<ActionResult> DisbandClan(DisbandClanRequestModel model)
        //        {
        //            var character = await this.mmoService.FindCharacterByCharacterIdAsync(model.CharacterId);
        //            if (character != null)
        //            {
        //                if (character.ClanId == 0)
        //                {
        //                    return Ok(new { Status = "Character is not in a clan" });
        //                }
        //                else
        //                {
        //                    var clan = await this.mmoService.FindClanByIdAsync(character.ClanId);
        //                    if (clan.LeaderId != model.CharacterId)
        //                    {
        //                        return Ok(new { Status = "Character is not the clan leader." });
        //                    }
        //                    else
        //                    {
        //                        var clanId = character.ClanId;
        //                        await this.mmoService.DeleteClanAsync(clanId);
        //                        await this.mmoService.UpdateCharactersClanAsync(clanId);
        //                        return Ok(new { Status = "OK" });
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "character not found" });
        //            }
        //        }

        //        [HttpPost]
        //        [Route(nameof(GetClanCharacters))]
        //        public async Task<ActionResult> GetClanCharacters()
        //        {
        //            var clans = await this.mmoService.GetClans();
        //            return Ok(new { Status = "OK", Clans = clans });
        //        }

        //        [HttpPost]
        //        [Route(nameof(DeleteCharacterFromClan))]
        //        public async Task<ActionResult> DeleteCharacterFromClan(DeleteCharacterFromClanRequestModel model)
        //        {
        //            var character = await this.mmoService.FindCharacterByCharacterIdAsync(model.CharacterId);
        //            if (character != null)
        //            {
        //                if (character.ClanId != 0)
        //                {
        //                    await this.mmoService.UpdateCharacterClanAsync(character, 0);
        //                    return Ok(new { Status = "OK" });
        //                }
        //                else
        //                {
        //                    return Ok(new { Status = "character is not in a clan" });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "character not found" });
        //            }
        //        }
    }
}
