using COO.Business.Logic.MMO.Write.Authentication;
using COO.Business.Logic.MMO.Write.ConfirmEmail;
using COO.Business.Logic.MMO.Write.CreateCharacter;
using COO.Business.Logic.MMO.Write.Registration;
using COO.Server.Controllers.MMO.Models;
using COO.Server.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using COO.Business.Logic.MMO.Write.DeleteCharacter;
using COO.Business.Logic.MMO.Write.GetCharacter;

namespace COO.Server.Controllers.MMO
{
    public class MMOController : ApiController
    {
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;

        public MMOController(
            IOptions<AppSettings> appSettings,
            IEmailService emailService,
            IMediator mediator)
        {
            _emailService = emailService;
            _mediator = mediator;
        }

        [HttpPost]
        [Route(nameof(Registration))]
        public async Task<ActionResult> Registration(RegistrationRequestModel model)
        {
            var user = await _mediator.Send(new RegistrationCommand(model.Login, model.Email, model.Password));

            var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "MMO",
                    new { userId = user.UserId, token = user.Token },
                    protocol: HttpContext.Request.Scheme);

            await _emailService.SendAsync(
                to: user.Email,
                subject: "ConfirmEmail",
                html: $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>"
                );

            return Ok(new { Message = $"The account was created successfully, a confirmation email {user.Email} was sent to your email." }) ;
        }

        [HttpGet]
        [Route(nameof(ConfirmEmail))]
        public async Task<ActionResult> ConfirmEmail(int userId, string token)
        {
            return Ok(await _mediator.Send(new ConfirmEmailCommand(userId, token)));
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(string login, string password)
        {
            return Ok(await _mediator.Send(new AuthenticationCommand(login, password)));
        }


        [HttpPost]
        [Route(nameof(CreateCharacter))]
        public async Task<ActionResult> CreateCharacter(CreateCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new CreateCharacterCommand(model.UserId, model.Token, model.Name, model.Gender, model.RaceId, model.ClassId)));
        }

        [HttpPost]
        [Route(nameof(DeleteCharacter))]
        public async Task<ActionResult> DeleteCharacter(DeleteCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new DeleteCharacterCommand(model.UserId, model.Token, model.CharacterId)));
        }

        [HttpPost]
        [Route(nameof(GetCharacter))]
        public async Task<ActionResult> GetCharacter(GetCharacterRequestModel model)
        {
            return Ok(await _mediator.Send(new GetCharacterCommand(model.UserId, model.Token, model.CharacterId)));
        }


        //        [HttpPost]
        //        [Route(nameof(GetCharacters))]
        //        public async Task<ActionResult> GetCharacters(GetCharactersRequestModel model)
        //        {
        //            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
        //            if (activeLogin != null)
        //            {
        //                // if the user owns the current active session
        //                if (activeLogin.SessionKey == model.SessionKey)
        //                {
        //                    var characters = await this.mmoService.GetCharacterListByUserIdAsync(model.UserId);

        //                    return Ok(new { Status = "OK", characters = characters });
        //                }
        //                else
        //                {
        //                    return Ok(new { Status = "You are not logged in." });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "You are not logged in." });
        //            }
        //        }

        //        [HttpPost]
        //        [Route(nameof(GetIP))]
        //        public async Task<ActionResult> GetIP()
        //        {
        //            var address = await this.mmoService.GetIPAsync();
        //#if DEBUG
        //            address = "127.0.0.1";
        //#endif
        //            return Ok(new { Status = "OK", Address = address });
        //        }

        //        [HttpPost]
        //        [Route(nameof(SaveCharacter))]
        //        public async Task<ActionResult> SaveCharacter(SaveCharacterRequestModel model)
        //        {
        //            await this.mmoService.UpdateCharacterAsync(
        //                model.CharacterId, model.Health, model.Mana, model.Experience,
        //                model.Level, model.PosX, model.PosY, model.PosZ,
        //                model.RotationYaw, model.EquipChest, model.EquipFeet,
        //                model.EquipHands, model.EquipHead, model.EquipLegs,
        //                model.Hotbar0, model.Hotbar1, model.Hotbar2, model.Hotbar3
        //                );

        //            await this.mmoService.DeleteRangeInventoryByCharacterIdAsync(model.CharacterId);

        //            await this.mmoService.DeleteRangeQuestsByCharacterIdAsync(model.CharacterId);

        //            if (model.Inventory.Count > 0)
        //            {
        //                var inventory = new List<Inventory>();
        //                model.Inventory.ForEach(i => {
        //                    inventory.Add(new Inventory {
        //                        CharacterId = model.CharacterId,
        //                        Slot = i.Slot,
        //                        Item = i.Item,
        //                        Amount = i.Amount
        //                    });
        //                });
        //                await this.mmoService.AddRangeInventoryAsync(inventory);
        //            }

        //            if (model.Quests.Count > 0)
        //            {
        //                var quests = new List<Quest>();
        //                model.Quests.ForEach(q => {
        //                    if (q.Completed)
        //                    {
        //                        quests.Add(new Quest
        //                        {
        //                            CharacterId = model.CharacterId,
        //                            Name = q.Name,
        //                            Completed = q.Completed,
        //                            Task1 = 0,
        //                            Task2 = 0,
        //                            Task3 = 0,
        //                            Task4 = 0
        //                        });
        //                    }
        //                    else
        //                    {
        //                        quests.Add(new Quest
        //                        {
        //                            CharacterId = model.CharacterId,
        //                            Name = q.Name,
        //                            Completed = q.Completed,
        //                            Task1 = q.Task1,
        //                            Task2 = q.Task2,
        //                            Task3 = q.Task3,
        //                            Task4 = q.Task4
        //                        });
        //                    }

        //                });
        //                await this.mmoService.AddRangeQuestsAsync(quests);
        //            }

        //            return Ok(new { Status = "OK" });
        //        }

        //        [HttpPost]
        //        [Route(nameof(GetServer))]
        //        public async Task<ActionResult> GetServer()
        //        {
        //            var address = await this.mmoService.GetServer();
        //#if DEBUG
        //            address = "127.0.0.1";
        //#endif
        //            return Ok(new { Status = "OK", Address = address });
        //        }

        //        [HttpPost]
        //        [Route(nameof(GetServerList))]
        //        public async Task<ActionResult> GetServerList(GetServerListRequestModel model)
        //        {
        //            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
        //            if (activeLogin != null)
        //            {
        //                // if the user owns the current active session
        //                if (activeLogin.SessionKey == model.SessionKey)
        //                {
        //                    var servers = await this.mmoService.GetServerList();

        //                    return Ok(new { Status = "OK", servers = servers });
        //                }
        //                else
        //                {
        //                    return Ok(new { Status = "You are not logged in." });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "You are not logged in." });
        //            }
        //        }

        //        [HttpPost]
        //        [Route(nameof(AddCharacterToClan))]
        //        public async Task<ActionResult> AddCharacterToClan(AddCharacterToClanRequestModel model)
        //        {
        //            var character = await this.mmoService.FindCharacterByNameAsync(model.CharacterName);
        //            if (character != null)
        //            {
        //                if (character.ClanId != 0)
        //                {
        //                    return Ok(new { Status = "character is already in a clan" });
        //                }
        //                else
        //                {
        //                    await this.mmoService.UpdateCharacterClanAsync(character, model.ClanId);
        //                    return Ok(new { Status = "OK" });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "character not found" });
        //            }
        //        }

        //        [HttpPost]
        //        [Route(nameof(CreateClan))]
        //        public async Task<ActionResult> CreateClan(CreateClanRequestModel model)
        //        {
        //            var clan = await this.mmoService.FindClanByNameAsync(model.ClanName);
        //            if (clan != null)
        //            {
        //                return Ok(new { ststus = "This clan name is unavailable" });
        //            }
        //            else
        //            {
        //                var character = await this.mmoService.FindCharacterByNameAsync(model.CharacterName);
        //                if (character != null)
        //                {
        //                    if (character.ClanId != 0)
        //                    {
        //                        return Ok(new { Status = "character already has a clan" });
        //                    }
        //                    else
        //                    {
        //                        var clanId = await this.mmoService.CreateClanAsync(character.Id, model.ClanName);

        //                        await this.mmoService.UpdateCharacterClanAsync(character, clanId);

        //                        return Ok(new { Status = "OK" });
        //                    }
        //                }
        //                else
        //                {
        //                    return Ok(new { Status = "character not found" });
        //                }
        //            }
        //        }

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

        //[HttpPost]
        //[Route(nameof(CheckClient))]
        //public async Task<ActionResult> CheckClient(CheckClientRequestModel model)
        //{
        //    var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
        //    if (activeLogin != null)
        //    {
        //        // if the user owns the current active session
        //        if (activeLogin.SessionKey == model.SessionKey)
        //        {
        //            // check that the character with this id belongs to this player
        //            var character = await this.mmoService.FindCharacterByCharacterIdAndUserIdAsync(model.CharacterId, model.UserId);
        //            if (character != null)
        //            {
        //                return Ok(new { Status = "OK" });
        //            }
        //            else
        //            {
        //                return Ok(new { Status = "Character not found" });
        //            }
        //        }
        //        else
        //        {
        //            return Ok(new { Status = "You are not logged in." });
        //        }
        //    }
        //    else
        //    {
        //        return Ok(new { Status = "You are not logged in." });
        //    }
        //}
    }
}
