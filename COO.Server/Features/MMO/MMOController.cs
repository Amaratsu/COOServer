namespace COO.Server.Features.MMO
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using COO.Server.Features.Identity.Models;
    using COO.Server.Features.MMO.Models;
    using COO.Server.Infrastructure.Helpers;
    using COO.Server.Infrastructure.Services;
    using COO.Server.Middleware;
    using Data.Models;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class MMOController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly IMMOService mmoService;
        private readonly AppSettings appSettings;
        private readonly IEmailService emailService;

        public MMOController(
            UserManager<User> userManager,
            IMMOService mmoService,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.mmoService = mmoService;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            RegisterValidator validator = new RegisterValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var newUser = await this.userManager.CreateAsync(user, model.Password);
                if (newUser.Succeeded)
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "MMO",
                            new { userId = user.Id, code = code },
                            protocol: HttpContext.Request.Scheme);

                    await this.emailService.SendAsync(
                        to: user.Email,
                        subject: "ConfirmEmail",
                        html: $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>"
                        );

                    return Ok(new { Status = "OK" });
                }
                else
                {
                    var errorMessage = "";
                    foreach (var error in newUser.Errors)
                    {
                        errorMessage = error.Description;
                    }

                    throw new AppException(errorMessage);
                }
            }
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(ConfirmEmail))]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new AppException("Link is incorrect");
            }
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new AppException("Link is incorrect");
            }
            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect($"{this.appSettings.ClientUrl}/login");
            }
            else
            {
                throw new AppException("Link is incorrect");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(LoginRequestModel model)
        {
            LoginValidator validator = new LoginValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {

                var user = await this.userManager.FindByNameAsync(model.Login);
                if (user == null)
                {
                    throw new AppException("Login or password is incorrect");
                }

                if (!await this.userManager.CheckPasswordAsync(user, model.Password))
                {
                    throw new AppException("Login or password is incorrect");
                }

                if (!await this.userManager.IsEmailConfirmedAsync(user))
                {
                    throw new AppException("Login or password is incorrect");
                }

                await this.mmoService.DeleteActiveLoginAsync(user.Id);

                var randomString = Helper.RandomString(10);

                await this.mmoService.CreateActiveLoginAsync(user.Id, randomString, null);

                return Ok(new { Status = "OK", SessionKey = randomString, UserId = user.Id });
            }
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CheckClient))]
        public async Task<ActionResult> CheckClient(CheckClientRequestModel model)
        {
            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
            if (activeLogin != null)
            {
                // if the user owns the current active session
                if (activeLogin.SessionKey == model.SessionKey)
                {
                    // check that the character with this id belongs to this player
                    var character = await this.mmoService.FindCharacterByCharacterIdAndUserIdAsync(model.CharacterId, model.UserId);
                    if (character != null)
                    {
                        return Ok(new { Status = "OK" });
                    }
                    else
                    {
                        return Ok(new { Status = "Character not found" });
                    }
                }
                else
                {
                    return Ok(new { Status = "You are not logged in." });
                }
            }
            else
            {
                return Ok(new { Status = "You are not logged in." });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CreateCharacter))]
        public async Task<ActionResult> CreateCharacter(CreateCharacterRequestModel model)
        {
            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
            if (activeLogin != null)
            {
                if (activeLogin.SessionKey == model.SessionKey)
                {
                    var character = await this.mmoService.FindCharacterByNameAsync(model.Name);
                    if (character == null)
                    {
                        // TEMPORARY
                        var gender = 0;
                        var health = 300;
                        var mana = 100;
                        var posx = 6890;
                        var posy = -3370;
                        var posz = 20692;
                        var yaw = 0;
                        await this.mmoService.CreateCharacterAsync(
                            model.UserId,
                            model.Name,
                            model.ClassId,
                            gender,
                            health,
                            mana,
                            posx,
                            posy,
                            posz,
                            yaw,
                            "",
                            "",
                            "",
                            "",
                            "",
                            "/Game/MMO/Abilities/DA_Heal.DA_Heal",
                            "/Game/MMO/Abilities/DA_FireBlast.DA_FireBlast",
                            "",
                            ""
                            );
                        return Ok(new { Status = "OK" });
                    }
                    else
                    {
                        return Ok(new { Status = "This name is unavailable" });
                    }
                }
                else
                {
                    return Ok(new { Status = "You are not logged in." });
                }
            }
            else
            {
                return Ok(new { Status = "You are not logged in." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DeleteCharacter))]
        public async Task<ActionResult> DeleteCharacter(DeleteCharacterRequestModel model)
        {
            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
            if (activeLogin != null)
            {
                // if the user owns the current active session
                if (activeLogin.SessionKey == model.SessionKey)
                {
                    if (!await this.mmoService.DeleteCharacterAsync(model.CharacterId))
                    {
                        return Ok(new { Status = "Character not found" });
                    }
                    else
                    {
                        return Ok(new { Status = "OK" });
                    }
                }
                else
                {
                    return Ok(new { Status = "You are not logged in." });
                }
            }
            else
            {
                return Ok(new { Status = "You are not logged in." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetCharacter))]
        public async Task<ActionResult> GetCharacter(GetCharacterRequestModel model)
        {
            var character = await this.mmoService.FindCharacterByCharacterIdAsync(model.CharacterId);
            if (character != null)
            {
                await this.mmoService.UpdateActiveLoginAsync(model.UserId, null, model.CharacterId);

                var inventories = await this.mmoService.GetInventoryListByCharacterIdAsync(model.CharacterId);

                var clan = await this.mmoService.FindClanByIdAsync(character.ClanId);

                var quests = await this.mmoService.GetQuestListByCharacterIdAsync(model.CharacterId);

                return Ok(new {
                    Status = "OK",
                    Name = character.Name,
                    Inventory = inventories,
                    Quests = quests,
                    Health = character.Health,
                    Mana = character.Mana,
                    Level = character.Level,
                    Experience = character.Experience,
                    ClanName = clan != null ? clan.Name : "",
                    PosX = character.PosX,
                    PosY = character.PosY,
                    PosZ = character.PosZ,
                    RotationYaw = character.RotationYaw,
                    EquipHead = character.EquipHead,
                    EquipChest = character.EquipChest,
                    EquipHands = character.EquipHands,
                    EquipLegs = character.EquipLegs,
                    EquipFeet = character.EquipFeet,
                    Hotbar0 = character.Hotbar0,
                    Hotbar1 = character.Hotbar1,
                    Hotbar2 = character.Hotbar2,
                    Hotbar3 = character.Hotbar3
                });
            }
            else
            {
                return Ok(new { Status = $"Character id {model.CharacterId} not found" });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetCharacters))]
        public async Task<ActionResult> GetCharacters(GetCharactersRequestModel model)
        {
            var activeLogin = await this.mmoService.FindActiveLoginAsync(model.UserId);
            if (activeLogin != null)
            {
                // if the user owns the current active session
                if (activeLogin.SessionKey == model.SessionKey)
                {
                    var characters = await this.mmoService.GetCharacterListByUserIdAsync(model.UserId);

                    return Ok(new { Status = "OK", characters = characters });
                }
                else
                {
                    return Ok(new { Status = "You are not logged in." });
                }
            }
            else
            {
                return Ok(new { Status = "You are not logged in." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetIP))]
        public async Task<ActionResult> GetIP()
        {
            var address = await this.mmoService.GetIPAsync();
#if DEBUG
            address = "127.0.0.1";
#endif
            return Ok(new { Status = "OK", Address = address });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(SaveCharacter))]
        public async Task<ActionResult> SaveCharacter(SaveCharacterRequestModel model)
        {
            await this.mmoService.UpdateCharacterAsync(
                model.CharacterId, model.Health, model.Mana, model.Experience,
                model.Level, model.PosX, model.PosY, model.PosZ,
                model.RotationYaw, model.EquipChest, model.EquipFeet,
                model.EquipHands, model.EquipHead, model.EquipLegs,
                model.Hotbar0, model.Hotbar1, model.Hotbar2, model.Hotbar3
                );

            await this.mmoService.DeleteRangeInventoryByCharacterIdAsync(model.CharacterId);

            await this.mmoService.DeleteRangeQuestsByCharacterIdAsync(model.CharacterId);

            if (model.Inventory.Count > 0)
            {
                var inventory = new List<Inventory>();
                model.Inventory.ForEach(i => {
                    inventory.Add(new Inventory {
                        CharacterId = model.CharacterId,
                        Slot = i.Slot,
                        Item = i.Item,
                        Amount = i.Amount
                    });
                });
                await this.mmoService.AddRangeInventoryAsync(inventory);
            }

            if (model.Quests.Count > 0)
            {
                var quests = new List<Quest>();
                model.Quests.ForEach(q => {
                    if (q.Completed)
                    {
                        quests.Add(new Quest
                        {
                            CharacterId = model.CharacterId,
                            Name = q.Name,
                            Completed = q.Completed,
                            Task1 = 0,
                            Task2 = 0,
                            Task3 = 0,
                            Task4 = 0
                        });
                    }
                    else
                    {
                        quests.Add(new Quest
                        {
                            CharacterId = model.CharacterId,
                            Name = q.Name,
                            Completed = q.Completed,
                            Task1 = q.Task1,
                            Task2 = q.Task2,
                            Task3 = q.Task3,
                            Task4 = q.Task4
                        });
                    }

                });
                await this.mmoService.AddRangeQuestsAsync(quests);
            }

            return Ok(new { Status = "OK" });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetServer))]
        public async Task<ActionResult> GetServer()
        {
            var address = await this.mmoService.GetServer();
#if DEBUG
            address = "127.0.0.1";
#endif
            return Ok(new { Status = "OK", Address = address });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(AddCharacterToClan))]
        public async Task<ActionResult> AddCharacterToClan(AddCharacterToClanRequestModel model)
        {
            var character = await this.mmoService.FindCharacterByNameAsync(model.CharacterName);
            if (character != null)
            {
                if (character.ClanId != 0)
                {
                    return Ok(new { Status = "character is already in a clan" });
                }
                else
                {
                    await this.mmoService.UpdateCharacterClanAsync(character, model.ClanId);
                    return Ok(new { Status = "OK" });
                }
            }
            else
            {
                return Ok(new { Status = "character not found" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CreateClan))]
        public async Task<ActionResult> CreateClan(CreateClanRequestModel model)
        {
            var clan = await this.mmoService.FindClanByNameAsync(model.ClanName);
            if (clan != null)
            {
                return Ok(new { ststus = "This clan name is unavailable" });
            }
            else
            {
                var character = await this.mmoService.FindCharacterByNameAsync(model.CharacterName);
                if (character != null)
                {
                    if (character.ClanId != 0)
                    {
                        return Ok(new { Status = "character already has a clan" });
                    }
                    else
                    {
                        var clanId = await this.mmoService.CreateClanAsync(character.Id, model.ClanName);

                        await this.mmoService.UpdateCharacterClanAsync(character, clanId);

                        return Ok(new { Status = "OK" });
                    }
                }
                else
                {
                    return Ok(new { Status = "character not found" });
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DisbandClan))]
        public async Task<ActionResult> DisbandClan(DisbandClanRequestModel model)
        {
            var character = await this.mmoService.FindCharacterByCharacterIdAsync(model.CharacterId);
            if (character != null)
            {
                if (character.ClanId == 0)
                {
                    return Ok(new { Status = "Character is not in a clan" });
                }
                else
                {
                    var clan = await this.mmoService.FindClanByIdAsync(character.ClanId);
                    if (clan.LeaderId != model.CharacterId)
                    {
                        return Ok(new { Status = "Character is not the clan leader." });
                    }
                    else
                    {
                        var clanId = character.ClanId;
                        await this.mmoService.DeleteClanAsync(clanId);
                        await this.mmoService.UpdateCharactersClanAsync(clanId);
                        return Ok(new { Status = "OK" });
                    }
                }
            }
            else
            {
                return Ok(new { Status = "character not found" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetClanCharacters))]
        public async Task<ActionResult> GetClanCharacters()
        {
            var clans = await this.mmoService.GetClans();
            return Ok(new { Status = "OK", Clans = clans });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DeleteCharacterFromClan))]
        public async Task<ActionResult> DeleteCharacterFromClan(DeleteCharacterFromClanRequestModel model)
        {
            var character = await this.mmoService.FindCharacterByCharacterIdAsync(model.CharacterId);
            if (character != null)
            {
                if (character.ClanId != 0)
                {
                    await this.mmoService.UpdateCharacterClanAsync(character, 0);
                    return Ok(new { Status = "OK" });
                }
                else
                {
                    return Ok(new { Status = "character is not in a clan" });
                }
            }
            else
            {
                return Ok(new { Status = "character not found" });
            }
        }
    }
}
