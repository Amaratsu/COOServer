namespace COO.Server.Features.Servers
{
    using COO.Server.Data.Models;
    using COO.Server.Features.Servers.Models;
    using COO.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using COO.Server.Infrastructure.Helpers;
    using Microsoft.Extensions.Options;

    public class ServersController : ApiController
    {
        private readonly IServerService servers;
        private readonly IEmailService emailService;
        private readonly AppSettings appSettings;

        public ServersController(
            IServerService servers,
            IOptions<AppSettings> appSettings,
            IEmailService emailService
            )
        {
            this.servers = servers;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserByName(model.Username);
                if (saveUser == null)
                {
                    saveUser = await this.servers.FindSaveUserByEmail(model.Email);
                }

                if (saveUser != null)
                {
                    if (saveUser.Username == model.Username)
                    {
                        return Ok("Error5");
                    }
                    else
                    {
                        return Ok("Error20");
                    }
                }
                else
                {
                    if (!Validators.IsValidEmail(model.Email))
                    {
                        return Ok("Error18");
                    }
                    else
                    {
                        var newSaveUser = new SaveUser
                        {
                            Username = model.Username,
                            Email = model.Email,
                            Verification = Converters.RandomString(16),
                            PasswordHash = Converters.CreateHashPassword(model.Password),
                            CharLimit = 4
                        };
                        await this.servers.InsertSavePlayer(newSaveUser);

                        saveUser = await this.servers.FindSaveUserByName(model.Username);

                        if (saveUser != null)
                        {
                            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Verification";

                            await this.emailService.SendAsync(
                                to: saveUser.Email,
                                subject: "Email Verification Test",
                                html: $"<h1>Thank you for trying my multiplayer system.</h1><p>Click the link to verify your account:<br>{host}?Email={saveUser.Email}&Verification={saveUser.Verification}'</p>",
                                appSettings.EmailFrom);

                            return Ok("Prompt0");
                        }
                        else
                        {
                            return Ok("Error17");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(LoginRequestModel model)
        {
            try
            {
                var ip = await this.servers.GetIP();

                var saveUser = await this.servers.FindSaveUserByNameOrEmail(model.Username);

                if (saveUser != null)
                {
                    var playUser = await this.servers.FindPlayUserById(saveUser.Id);
                    if (Converters.CheckPassword(model.Password, saveUser.PasswordHash) || model.IsPlatform == 1)
                    {
                        if (saveUser.Verification == "1")
                        {
                            if (!playUser.IsLogin)
                            {
                                if (saveUser.PrevIP == ip || saveUser.PrevIP == "" || model.IsPlatform == 1)
                                {
                                    saveUser.PrevIP = ip;
                                    saveUser.PrevLogin = "";
                                    saveUser.PrevDevice = model.PreDevice;
                                    await this.servers.UpdateSaveUser();
                                    playUser.IsLogin = true;
                                    playUser.CurrentChar = "";
                                    if (playUser.Alert.IndexOf("Login:" + model.PreDevice + "|") == -1)
                                    {
                                        playUser.Alert = playUser.Alert + "Login:" + model.PreDevice + "|";
                                        await this.servers.UpdatePlayUser();
                                    }
                                    return Ok(saveUser.Id + "/" + saveUser.Username);
                                }
                                else
                                {
                                    saveUser.Verification = Converters.RandomString(4);
                                    await this.servers.UpdatePlayUser();

                                    var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Verification";

                                    await this.emailService.SendAsync(
                                        to: saveUser.Email,
                                        subject: "Email Verification Test",
                                        html: $"<h1> Different IP from previous login.</h1><p>Click the link to re - verify your account:<br>{host}?Email={saveUser.Email}&Verification={saveUser.Verification}'</p>");

                                    return Ok("Error4");
                                }
                            }
                            else
                            {
                                saveUser.PrevDevice = model.PreDevice;
                                await this.servers.UpdateSaveUser();

                                playUser.IsLogin = false;
                                await this.servers.UpdatePlayUser();

                                if (playUser.Alert.IndexOf("Login:" + model.PreDevice + "|") == -1)
                                {
                                    playUser.Alert = playUser.Alert + "Login:" + model.PreDevice + "|";
                                    await this.servers.UpdatePlayUser();
                                }

                                return Ok("Error3");
                            }
                        }
                        else
                        {
                            if (saveUser.Verification.Length < 16)
                            {
                                return Ok("Prompt4");
                            }
                            else {
                                return Ok("Prompt0");
                            }
                        }
                    }
                    else
                    {
                        return Ok("Error2");
                    }
                } 
                else if (model.IsPlatform == 1)
                {
                    var newSaveUser = new SaveUser
                    {
                        Username = model.Username,
                        Verification = "1",
                        CharLimit = 4
                    };

                    await this.servers.UpdateSaveUser();

                    saveUser = await this.servers.FindSaveUserByNameOrEmail(model.Username);

                    if (saveUser != null)
                    {
                        var newPlayUser = new PlayUser
                        {
                            Id = saveUser.Id,
                            Username = saveUser.Username,
                            IsLogin = false
                        };
                        await this.servers.UpdatePlayUser();

                        return Ok(saveUser.Id + "/" + saveUser.Username);
                    }
                    else
                    {
                        return Ok("Error15");
                    }
                }
                else
                {
                    return Ok("Error1");
                }
            } 
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Logout))]
        public async Task<ActionResult> Logout(LogoutRequestModel model)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserById(model.UserId);
                if (saveUser != null)
                {
                    saveUser.PrevLogin = model.DateTime;
                    await this.servers.UpdateSaveUser();

                    var playUser = await this.servers.FindPlayUserById(saveUser.Id);
                    if (playUser != null)
                    {
                        playUser.IsLogin = false;
                        playUser.MainIP = "";
                        playUser.InstanceIP = "";
                        playUser.Alert = "";
                        playUser.XServerMessages = "";
                        playUser.GIReady = "";
                    }
                    await this.servers.UpdatePlayUser();
                }

                return Ok("Success2");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(VerificationResend))]
        public async Task<ActionResult> VerificationResend(string userName)
        {
            try 
            {
                var saveUser = await this.servers.FindSaveUserByNameOrEmail(userName);
                if (saveUser != null)
                {
                    if (saveUser.Verification == "1")
                    {
                        return Ok("Success1");
                    }
                    else
                    {
                        var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Verification";

                        await this.emailService.SendAsync(
                            to: saveUser.Email,
                            subject: "Email Verification Test",
                            html: $"<h1>Thank you for trying my multiplayer system.</h1><p>Click the link to verify your account:<br>{host}?Email={saveUser.Email}&Verification={saveUser.Verification}'</p>");

                        return Ok("Success0");
                    }
                }
                else
                {
                    return Ok("Error14");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetUsersInfo))]
        public async Task<ActionResult> GetUsersInfo(int userId)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserById(userId);
                return Ok($"{saveUser.FavServers}/{saveUser.CharLimit}/{saveUser.FriendList}/{saveUser.BlockedList}/{saveUser.BankInv}");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetCharList))]
        public async Task<ActionResult> GetCharList(int userId)
        {
            try
            {
                var charUsers = await this.servers.GetCharListById(userId);
                if (charUsers.Count > 0)
                {
                    var response = "";
                    foreach(var cu in charUsers)
                    {
                        response += $"{cu.Server}/{cu.Name}/{cu.Affiliation}/{cu.XP}/{cu.Status}/{cu.Inv}/{cu.Equips}/{cu.Skills}/{cu.Talents}/{cu.Appearance}/{cu.Gameplay}/{cu.Keybinds}/{cu.KeyRemap}/{cu.Chat};";
                    }
                    return Ok(response);
                }
                else
                {
                    return Ok("Success3");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CreateChar))]
        public async Task<ActionResult> CreateChar(CreateCharRequestModel model)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserById(model.UserId);
                if (saveUser != null && saveUser.PrevDevice == model.PrevDevice)
                {
                    var charUser = await this.servers.FindCharUserByName(model.Name);
                    if (charUser == null)
                    {
                        var newCharUser = new CharUser
                        {
                            Id = model.UserId,
                            Server = "",
                            Name = model.Name,
                            Affiliation = model.Affiliation,
                            XP = model.XP,
                            Status = model.Status,
                            Inv = model.Inv,
                            Equips = model.Equips,
                            Skills = model.Skills,
                            Talents = model.Talents,
                            Appearance = model.Appearance,
                            Gameplay = model.Gameplay,
                            Keybinds = model.Keybinds,
                            KeyRemap = model.KeyRemap,
                            Chat = model.Chat
                        };
                        await this.servers.UpdatePlayUser();

                        return Ok("Success4");
                    }
                    else
                    {
                        return Ok("Success6");
                    }
                }
                else
                {
                    return Ok("Prompt2");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DeleteChar))]
        public async Task<ActionResult> DeleteChar(DeleteCharRequestModel model)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserById(model.UserId);
                if (saveUser != null && saveUser.PrevDevice == model.PrevDevice)
                {
                    var charUser = await this.servers.FindCharUserByName(model.Name);
                    await this.servers.DeleteCharUser(charUser);

                    return Ok("Success8");
                }
                else
                {
                    return Ok("Prompt2");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetServerList))]
        public async Task<ActionResult> GetServerList(string serverType)
        {
            try
            {
                var response = "";

                var servers = await this.servers.GetServerList(serverType);

                if (servers.Count > 0)
                {
                    foreach (var s in servers)
                    {
                        response += $"{s.ServerType}/{s.IP}/{s.Port}/{s.Name}/{s.Password}/{s.Region}/{s.IsInGame}/{s.CNP}/{s.MNP}/{s.PG}/{s.IG};";
                    }
                }
                else
                {
                    return Ok("Error12");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(UpdateAccount))]
        public async Task<ActionResult> UpdateAccount(UpdateAccountRequestModel model)
        {
            try
            {
                var saveUser = await this.servers.FindSaveUserById(model.UserId);
                if (saveUser != null && saveUser.PrevDevice == model.PrevDevice)
                {
                    saveUser.FavServers = model.FavServers;
                    saveUser.CharLimit = model.CharLimit;
                    saveUser.FriendList = model.FriendList;
                    saveUser.BlockedList = model.BlockedList;
                    saveUser.BankInv = model.BankInv;
                    await this.servers.UpdateSaveUser();

                    var charUser = await this.servers.FindCharUserByIdAndName(model.UserId, model.Name);
                    if (charUser != null)
                    {
                        charUser.Server = model.Server;
                        charUser.Name = model.Name;
                        charUser.Affiliation = model.Affiliation;
                        charUser.XP = model.XP;
                        charUser.Status = model.Status;
                        charUser.Inv = model.Inv;
                        charUser.Equips = model.Equips;
                        charUser.Skills = model.Skills;
                        charUser.Talents = model.Talents;
                        charUser.Appearance = model.Appearance;
                        charUser.Gameplay = model.Gameplay;
                        charUser.Keybinds = model.Keybinds;
                        charUser.KeyRemap = model.KeyRemap;
                        charUser.Chat = model.Chat;
                        await this.servers.UpdateSaveUser();
                    }

                    if (model.IsCharSelected)
                    {
                        var playUser = await this.servers.FindPlayUserById(model.UserId);
                        if (playUser != null)
                        {
                            playUser.CurrentChar = model.Name;
                            await this.servers.UpdatePlayUser();
                        }
                    }

                    return Ok("Success5");
                }
                else
                {
                    return Ok("Prompt2");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(AddServer))]
        public async Task<ActionResult> AddServer(AddServerRequestModel model)
        {
            try
            {
                var result = await this.servers.AddServer(
                    model.ServerType,
                    model.IP,
                    model.Port
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CheckHosts))]
        public async Task<ActionResult> CheckHosts(CheckHostsRequestModel model)
        {
            try
            {
                var response = "";
                var GIIPPort = "";
                var hostInfos = await this.servers.GetDS_HostInfos();
                if (hostInfos.Count > 0)
                {
                    var isFirst = true;
                    for(var i = 0; i < hostInfos.Count; i++)
                    {
                        if (isFirst && hostInfos[i].ServerType == model.ServerType)
                        {
                            var server = await this.servers.FindServerByIPPort(model.IP, model.Port);
                            if (server != null)
                            {
                                server.Name = hostInfos[i].Name;
                                server.Region = hostInfos[i].Region;
                                server.MNP = hostInfos[i].MNP;
                                server.PG = hostInfos[i].PG;
                                server.IG = hostInfos[i].IG;
                                await this.servers.UpdatePlayUser();

                                GIIPPort = model.IP + ":" + model.Port;

                                if (hostInfos[i].Password == "")
                                {
                                    response = "Public/";
                                }
                                else
                                {
                                    response = "Private/";
                                }

                                response += $"{hostInfos[i].Name}/{hostInfos[i].Password}/{hostInfos[i].Region}/{hostInfos[i].MNP}/{hostInfos[i].PG}/{hostInfos[i].IG}";

                                if (model.ServerType == "GI")
                                {
                                    var hosts = hostInfos[i].Hosts.Split("|");
                                    foreach(var h in hosts)
                                    {
                                        var playUser = await this.servers.FindPlayUserByUserName(h);
                                        if (playUser != null)
                                        {
                                            playUser.GIReady = GIIPPort;
                                            if (playUser.Alert.IndexOf("GIReady") == -1)
                                            {
                                                playUser.Alert += "GIReady|";
                                                await this.servers.UpdatePlayUser();
                                            }
                                        }
                                    }
                                }

                                await this.servers.DeleteDS_HostInfo(hostInfos[i]);

                                isFirst = false;
                                break;
                            }
                        }
                    }
                    
                    if (isFirst)
                    {
                        return Ok("Error7");
                    }

                    return Ok(response);
                }
                else
                {
                    return Ok("Error7");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(SetServerPassword))]
        public async Task<ActionResult> SetServerPassword(SetServerPasswordRequestModel model)
        {
            try
            {
                var server = await this.servers.FindServerByIPPort(model.IP, model.Port);
                if (server != null)
                {
                    server.Password = model.Password;
                    await this.servers.UpdatePlayUser();
                }

                return Ok("Success7");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(RemoveServer))]
        public async Task<ActionResult> RemoveServer(RemoveServerRequestModel model)
        {
            var result = await this.servers.RemoveServer(model.IP, model.Port);

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(PostHostRequest))]
        public async Task<ActionResult> PostHostRequest(PostHostRequestModel model)
        {
            var result = await this.servers.PostHostRequest(
                model.Hosts,
                model.ServerType,
                model.Name,
                model.Password,
                model.Region,
                model.MNP,
                model.PG,
                model.IG
                );
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DoubleCheckServer))]
        public async Task<ActionResult> DoubleCheckServer(DoubleCheckServerRequestModel model)
        {
            var result = await this.servers.DoubleCheckServer(
                    model.IP,
                    model.Port
                );

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CloudMM))]
        public async Task<ActionResult> CloudMM(CloudMMRequestModel model)
        {
            try
            {
                var lfgs = await this.servers.GetLFGs();
                if (lfgs.Count == 0 || (lfgs.Count > 0 && !lfgs.Exists(l => l.Members == model.Members && l.IsCanceled == model.IsCanceled)))
                {
                    var lfg = new LFG
                    {
                        Requester = model.Requester,
                        Members = model.Members,
                        HostRequest = model.HostRequest,
                        GameType = model.GameType,
                        TeamCount = model.TeamCount,
                        MNP = model.MNP,
                        IsCanceled = model.IsCanceled
                    };
                    await this.servers.AddLFG(lfg);
                }

                return Ok(model.IsCanceled ? "Success11" : "Success9");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetAlert))]
        public async Task<ActionResult> GetAlert(string fullIP)
        {
            try
            {
                var result = "";
                var playUsers = await this.servers.GetPlayUsers();
                if (playUsers.Count > 0)
                {
                    playUsers.ForEach(pu => {
                        if (pu.Alert != "")
                        {
                            result += pu.Username + "/" + pu.Alert + ";";
                            pu.Alert = "";
                            this.servers.UpdatePlayUser();
                        }
                    });
                    return Ok(result);
                }
                else
                {
                    return Ok("Success10");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(PartyInvX))]
        public async Task<ActionResult> PartyInvX(PartyInvXRequestModel model)
        {
            try
            {
                if (!model.IsSent)
                {
                    var playUser = await this.servers.FindPlayUserByUserName(model.Invitee);
                    if (playUser != null)
                    {
                        if (playUser.Alert.IndexOf("Party:Recv:" + model.Inviter + "|") == -1)
                        {
                            playUser.Alert += "Party:Recv:" + model.Inviter + "|";
                            await this.servers.UpdatePlayUser();
                        }
                    }
                }
                else
                {
                    if (!model.IsUpdateSent)
                    {
                        var membersArray = model.Members.Split(",");
                        if (membersArray.Length > 0)
                        {
                            foreach (var m in membersArray) {
                                var playUser = await this.servers.FindPlayUserByUserName(m);

                                if (playUser.Alert.IndexOf("Party:Sent:" + model.Invitee + "|") == -1)
                                {
                                    playUser.Alert += "Party:Sent:" + model.Invitee + "|";
                                    await this.servers.UpdatePlayUser();
                                }
                            };
                        }
                    }
                    else
                    {
                        var membersArray = model.Members.Split(",");
                        if (membersArray.Length > 0)
                        {
                            foreach (var m in membersArray)
                            {
                                var playUser = await this.servers.FindPlayUserByUserName(m);

                                if (playUser.Alert.IndexOf("Party:Sent:" + model.Invitee + "|") == -1)
                                {
                                    playUser.Alert += "Party:Sent:" + model.Invitee + "|";
                                    await this.servers.UpdatePlayUser();
                                }
                            };
                        }
                    }
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(PartyDecX))]
        public async Task<ActionResult> PartyDecX(PartyDecXRequestModel model)
        {
            try
            {
                if (!model.IsCancel)
                {
                    var invitorArray = model.Members.Split(",");
                    foreach(var ia in invitorArray)
                    {
                        var inviteePlayUser = await this.servers.FindPlayUserByUserName(model.Invitee);
                        var invitorPlayUser = await this.servers.FindPlayUserByUserName(ia);
                        if (invitorPlayUser != null)
                        {
                            if (inviteePlayUser.Alert.IndexOf("Party:Dec:0:0:" + ia + "::1|") == -1)
                            {
                                if (!model.IsAccepted)
                                {
                                    if (invitorPlayUser.Alert.IndexOf("Party:Dec:1:1:" + model.Invitee + ":" + model.IsCancel + "|") > -1)
                                    {
                                        var newAlert = invitorPlayUser.Alert.Replace("Party:Dec:1:1:" + model.Invitee + ":" + model.IsCancel +"|", "Party: Dec:0:1:" + model.Invitee + ":" + model.IsCancel + "|");
                                        invitorPlayUser.Alert = newAlert;
                                        await this.servers.UpdatePlayUser();
                                    }
                                    else if (invitorPlayUser.Alert.IndexOf("Party:Dec:" + model.IsAccepted + ":1:" + model.Invitee + ":" + model.IsCancel + "|") == -1)
                                    {
                                        invitorPlayUser.Alert += "Party:Dec:" + model.IsAccepted + ":1:" + model.Invitee + ":" + model.IsCancel + "|";
                                        await this.servers.UpdatePlayUser();
                                    }
                                }
                                else
                                {
                                    if (invitorPlayUser.Alert.IndexOf("Party:Dec:0:1:" + model.Invitee + ":" + model.IsCancel + "|") > -1)
                                    {
                                        var newAlert = invitorPlayUser.Alert.Replace("Party:Dec:0:1:" + model.Invitee + ":" + model.IsCancel + "|", "Party: Dec:1:1:" + model.Invitee + ":" + model.IsCancel + "|");
                                        invitorPlayUser.Alert = newAlert;
                                        await this.servers.UpdatePlayUser();
                                    }
                                    else if (invitorPlayUser.Alert.IndexOf("Party:Dec:" + model.IsAccepted + ":1:" + model.Invitee + ":" + model.IsCancel + "|") == -1)
                                    {
                                        invitorPlayUser.Alert += "Party:Dec:" + model.IsAccepted + ":1:" + model.Invitee + ":" + model.IsCancel + "|";
                                        await this.servers.UpdatePlayUser();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var inviteePlayUser = await this.servers.FindPlayUserByUserName(model.Invitee);
                    var invitorPlayUser = await this.servers.FindPlayUserByUserName(model.Inviter);
                }

                return Ok("");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(PartyLeaveX))]
        public async Task<ActionResult> PartyLeaveX(PartyLeaveXRequestModel model)
        {
            try
            {
                var leaverPlayUser = await this.servers.FindPlayUserByUserName(model.Leaver);
                if (leaverPlayUser != null)
                {
                    var membersArray = model.Members.Split(",");
                    foreach(var m in membersArray)
                    {
                        var member = await this.servers.FindPlayUserByUserName(m);

                        if(member != null && member.Alert.IndexOf("Party:Left::" + model.Leaver + ":" + model.NewLeader + "|") == -1)
                        {
                            member.Alert += "Party:Left:" + model.Leaver + ":" + model.NewLeader + "|";
                            await this.servers.UpdatePlayUser();
                        }
                    }
                }

                return Ok("");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(PartyUpdateDB))]
        public async Task<ActionResult> PartyUpdateDB(PartyUpdateDBRequestModel model)
        {
            try
            {
                if (model.Members == "")
                {
                    var playUser = await this.servers.FindPlayUserByUserName(model.Username);
                    if (playUser != null)
                    {
                        playUser.Leader = "";
                        playUser.CurrentParty = "";
                        await this.servers.UpdatePlayUser();
                    }
                }
                else
                {
                    var membersArray = model.Members.Split(",");
                    foreach (var m in membersArray)
                    {
                        var playUser = await this.servers.FindPlayUserByUserName(m);
                        if (playUser != null)
                        {
                            playUser.Leader = model.Leader;
                            playUser.CurrentParty = model.Members;
                            await this.servers.UpdatePlayUser();
                        }
                    }
                }
               
                return Ok("");
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(SetCNP))]
        public async Task<ActionResult> SetCNP(SetCNPRequestModel model)
        {
            try
            {
                var server = await this.servers.FindServerByIPPort(model.IP, model.Port);
                if (server != null)
                {
                    server.CNP = model.NumPlayer;
                    await this.servers.UpdatePlayUser();

                    return Ok("Success14");
                }
                else
                {
                    return Ok("Success12");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(SetMyServerIP))]
        public async Task<ActionResult> SetMyServerIP(SetMyServerIPRequestModel model)
        {
            var result = await this.servers.SetMyServerIP(
                model.Username,
                model.IP,
                model.IsIstance
                );

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(GetGIReady))]
        public async Task<ActionResult> GetGIReady(string Username)
        {
            try
            {
                var server = await this.servers.FindPlayUserByUserName(Username);
                if (server != null)
                {
                    return Ok(server.GIReady);
                }
                else
                {
                    return Ok("");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error0");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DS_GetHostRequest))]
        public async Task<ActionResult> DS_GetHostRequest(string Region)
        {
            var result = await this.servers.DS_GetHostRequest(Region);

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DS_PostHostRequest))]
        public async Task<ActionResult> DS_PostHostRequest(DS_PostHostRequestModel model)
        {
            var result = await this.servers.DS_PostHostRequest(
                model.Hosts,
                model.Name,
                model.Region,
                model.MNP,
                model.ServerType
            );

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(DS_ResetServers))]
        public async Task<ActionResult> DS_ResetServers(string IsStarted)
        {
            var result = await this.servers.DS_ResetServers();

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetIP))]
        public async Task<ActionResult> GetIP()
        {
            var IP = await this.servers.GetIP();

            return Ok(IP);
        }
    }
}
