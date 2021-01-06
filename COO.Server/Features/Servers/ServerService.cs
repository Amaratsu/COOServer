namespace COO.Server.Features.Servers
{
    using COO.Server.Data;
    using System.Threading.Tasks;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System.Net.Http;
    using System;

    public class ServerService : IServerService
    {
        private readonly COODbContext data;

        public ServerService(COODbContext data) => this.data = data;

        public async Task<string> AddServer(string ServerType, string IP, string Port)
        {
            try
            {

                var server = await FindServerByIpPort(IP, Port);

                if (server != null)
                {
                    return "Error10";
                }

                server = new Server
                {
                    ServerType = ServerType,
                    IP = IP,
                    Port = Port,
                    Name = "",
                    Password = "Thienhoang1",
                    Region = "",
                    IsInGame = false,
                    CNP = 0,
                    MNP = 0,
                    PG = "",
                    IG = ""
                };

                this.data.Servers.Add(server);

                await this.data.SaveChangesAsync();

                return "Success13";
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> DoubleCheckServer(string IP, string Port)
        {
            try
            {
                var server = await FindServerByIpPort(IP, Port);
                if (server != null)
                {
                    if (server.CNP >= server.MNP)
                    {
                        return "Error11";
                    }
                    else
                    {
                        return $"{server.IP}:{server.Port}";
                    }
                }
                else
                {
                    return "Error12";
                }

            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> DS_ResetServers()
        {
            try
            {
                var creationRequests = await GetCreationRequests();
                this.data.DS_CreationRequests.RemoveRange(creationRequests);

                var hostInfos = await GetHostInfos();
                this.data.DS_HostInfos.RemoveRange(hostInfos);

                var loginRequests = await GetLoginRequests();
                this.data.DS_LoginRequests.RemoveRange(loginRequests);

                var servers = await GetServers();
                this.data.Servers.RemoveRange(servers);

                var lFGs = await GetLFGs();
                this.data.LFGs.RemoveRange(lFGs);

                await this.data.SaveChangesAsync();

                return "Cleared";
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<List<Server>> GetServerList(string serverType)
            => await this.data
                .Servers
                .Where(s => s.ServerType == serverType && s.Password != "Thienhoang1")
                .ToListAsync();

        public async Task<string> RemoveServer(string IP, string Port)
        {
            try
            {
                var server = await FindServerByIpPort(IP, Port);

                if (server != null)
                {
                    this.data.Servers.Remove(server);
                    await this.data.SaveChangesAsync();
                }

                return "";
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> SetMyServerIP(string Username, string IP, bool IsInstance)
        {
            try
            {
                var foundPlayUser = await FindPlayUserByUserName(Username);

                if (foundPlayUser != null)
                {
                    if (IsInstance)
                    {
                        foundPlayUser.InstanceIP = IP;
                    }
                    else
                    {
                        foundPlayUser.MainIP = IP;
                    }
                    await this.data.SaveChangesAsync();
                }

                return IP;
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> DS_GetHostRequest(string Region)
        {
            try
            {
                var creationRequests = await GetCreationRequestsByRegion(Region);
                if (creationRequests.Count > 0)
                {
                    string result = "";

                    foreach (var request in creationRequests)
                    {
                        result += request.ServerType + ";";
                    }

                    this.data.DS_CreationRequests.RemoveRange(creationRequests);

                    await this.data.SaveChangesAsync();

                    return result;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> DS_PostHostRequest(string Hosts, string Name, string Region, int MNP, string ServerType)
        {
            int curNumber = 0;
            int nextNumber = 0;
            try
            {
                var servers = await GetServersByName(Name);

                if (servers.Count > 0)
                {
                    return "Server Name Taken";
                }
                else
                {
                    servers = await GetServers();

                    if (servers.Count > 0)
                    {
                        servers.ForEach(s =>
                        {
                            if (s.ServerType == ServerType)
                            {
                                if (s.Name == "Official" || s.Name == "Instance")
                                {
                                    curNumber = int.Parse(s.Name.Replace(Name, " "));
                                    if (curNumber - nextNumber <= 1)
                                    {
                                        nextNumber = int.Parse(s.Name.Replace(Name, " "));
                                    }
                                }
                            }
                        });
                    }
                    Name = Name + " " + (nextNumber + 1);

                    var hostInfo = new DS_HostInfo
                    {
                        Hosts = Hosts,
                        ServerType = ServerType,
                        Name = Name,
                        Password = "Thienhoang1",
                        Region = Region,
                        MNP = MNP,
                        PG = "",
                        IG = ""
                    };

                    this.data.DS_HostInfos.Add(hostInfo);

                    await this.data.SaveChangesAsync();

                    return "Game Posted";
                }
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }
        public async Task<string> PostHostRequest(string Hosts, string ServerType, string Name, string Password, string Region, int MNP, string PG, string IG)
        {
            int curNumber = 0;
            int nextNumber = 0;
            try
            {
                var servers = await GetServersByServerType(ServerType);

                if (servers.Count > 0) {
                    servers.ForEach(s =>
                    {
                        if (s.ServerType == ServerType)
                        {
                            if (s.Name == "Official" || s.Name == "Instance")
                            {
                                curNumber = int.Parse(s.Name.Replace(Name, " "));
                                if (curNumber - nextNumber <= 1)
                                {
                                    nextNumber = int.Parse(s.Name.Replace(Name, " "));
                                }
                            }
                        }
                    });

                    Name = Name + " " + (nextNumber + 1);
                }

                var ds_hostInfo = new DS_HostInfo
                {
                    Hosts = Hosts,
                    ServerType = ServerType,
                    Name = Name,
                    Password = Password,
                    Region = Region,
                    MNP = MNP,
                    PG = PG,
                    IG = IG
                };
                this.data.DS_HostInfos.Add(ds_hostInfo);

                var ds_creationRequest = new DS_CreationRequest
                {
                    Region = Region,
                    ServerType = ServerType
                };
                this.data.DS_CreationRequests.Add(ds_creationRequest);

                await this.data.SaveChangesAsync();

                return "Success6";
            }
            catch (Exception ex)
            {
                return "Error0";
            }
        }

        public async Task<string> GetIP()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api.ipify.org");
        }

        private async Task<Server> GetServerPlayerJoined(string ip, string port)
            => await this.data
                .Servers
                .Where(s => s.IP == ip && s.Port == port)
                .FirstOrDefaultAsync();

        private async Task<List<Server>> GetServers()
            => await this.data
                .Servers
                .ToListAsync();

        private async Task<List<Server>> GetServersByName(string Name)
            => await this.data
                .Servers
                .Where(s => s.Name == Name)
                .ToListAsync();

        private async Task<List<DS_CreationRequest>> GetCreationRequests()
            => await this.data
                .DS_CreationRequests
                .ToListAsync();

        private async Task<List<DS_CreationRequest>> GetCreationRequestsByRegion(string Region)
            => await this.data
                .DS_CreationRequests
                .Where(c => c.Region == Region)
                .ToListAsync();

        private async Task<List<DS_HostInfo>> GetHostInfos()
            => await this.data
                .DS_HostInfos
                .ToListAsync();

        private async Task<List<DS_LoginRequest>> GetLoginRequests()
            => await this.data
                .DS_LoginRequests
                .ToListAsync();

        public async Task<List<LFG>> GetLFGs()
            => await this.data
                .LFGs
                .ToListAsync();

        public async Task UpdatePlayUser()
        {
            await this.data.SaveChangesAsync();
        }

        public async Task UpdateSaveUser()
        {
            await this.data.SaveChangesAsync();
        }

        public async Task<SaveUser> FindSaveUserByName(string Username)
            => await this.data
                .SaveUsers
                .FirstOrDefaultAsync(s => s.Username == Username);

        public async Task<SaveUser> FindSaveUserByEmail(string Email)
            => await this.data
                .SaveUsers
                .FirstOrDefaultAsync(s => s.Email == Email);

        public async Task<SaveUser> FindSaveUserByNameOrEmail(string Username)
            => await this.data
                .SaveUsers
                .FirstOrDefaultAsync(s => s.Username == Username || s.Email == Username);

        public async Task<PlayUser> FindPlayUserById(int Id)
            => await this.data
                .PlayUsers
                .FirstOrDefaultAsync(s => s.Id == Id);

        public async Task<SaveUser> FindSaveUserById(int Id)
            => await this.data
                .SaveUsers
                .FirstOrDefaultAsync(s => s.Id == Id);

        public async Task<List<CharUser>> GetCharListById(int Id)
            => await this.data
                .CharUsers
                .Where(c => c.Id == Id)
                .ToListAsync();

        public async Task<CharUser> FindCharUserByName(string Name)
            => await this.data
                .CharUsers
                .FirstOrDefaultAsync(cu => cu.Name == Name);

        public async Task DeleteCharUser(CharUser CharUser)
        {
            this.data.CharUsers.Remove(CharUser);
            await this.data.SaveChangesAsync();
        }

        public async Task<CharUser> FindCharUserByIdAndName(int Id, string Name)
            => await this.data
                .CharUsers
                .FirstOrDefaultAsync(cu => cu.Id == Id && cu.Name == Name);

        public async Task<Server> FindServerByIpPort(string IP, string Port)
            => await this.data
                .Servers
                .FirstOrDefaultAsync(s => s.IP == IP && s.Port == Port);

        public async Task<List<DS_HostInfo>> GetDS_HostInfos()
            => await this.data
                .DS_HostInfos
                .ToListAsync();

        public async Task<PlayUser> FindPlayUserByUserName(string Username)
            => await this.data
                .PlayUsers
                .FirstOrDefaultAsync(pu => pu.Username == Username);

        public async Task DeleteDS_HostInfo(DS_HostInfo DS_HostInfo)
        {
            this.data.DS_HostInfos.Remove(DS_HostInfo);
            await this.data.SaveChangesAsync();
        }

        public async Task<List<Server>> GetServersByServerType(string ServerType)
            => await this.data
                .Servers
                .Where(s => s.ServerType == ServerType)
                .ToListAsync();

        public async Task AddLFG(LFG lFG)
        {
            this.data.LFGs.Add(lFG);
            await this.data.SaveChangesAsync();
        }

        public async Task<List<PlayUser>> GetPlayUsers()
            => await this.data
                .PlayUsers
                .ToListAsync();

        public async Task<Server> FindServerByIPPort(string IP, string Port)
            => await this.data
                .Servers
                .FirstOrDefaultAsync(s => s.IP == IP && s.Port == Port);

        public async Task InsertSavePlayer(SaveUser SaveUser)
        {
            this.data.SaveUsers.Add(SaveUser);
            await this.data.SaveChangesAsync();
        }
    }
}
