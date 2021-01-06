namespace COO.Server.Features.Servers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;

    public interface IServerService
    {
        Task<string> AddServer(string ServerType, string IP, string Port);
        Task<string> DoubleCheckServer(string IP, string Port);
        Task<string> DS_ResetServers();
        Task<List<Server>> GetServerList(string ServerType);
        Task<Server> FindServerByIPPort(string IP, string Port);
        Task<string> RemoveServer(string IP, string Port);
        Task<string> SetMyServerIP(string Username, string IP, bool IsInstance);
        Task<string> DS_GetHostRequest(string Region);
        Task<string> DS_PostHostRequest(string Hosts, string Name, string Region, int MNP, string ServerType);
        Task<PlayUser> FindPlayUserById(int Id);
        Task InsertSavePlayer(SaveUser SaveUser);
        Task UpdateSaveUser();
        Task UpdatePlayUser();
        Task<SaveUser> FindSaveUserByName(string Username);
        Task<SaveUser> FindSaveUserByEmail(string Email);
        Task<SaveUser> FindSaveUserByNameOrEmail(string Username);
        Task<SaveUser> FindSaveUserById(int Id);
        Task<List<CharUser>> GetCharListById(int Id);
        Task<CharUser> FindCharUserByName(string Name);
        Task DeleteCharUser(CharUser charUser);
        Task<CharUser> FindCharUserByIdAndName(int Id, string Name);
        Task<List<DS_HostInfo>> GetDS_HostInfos();
        Task<PlayUser> FindPlayUserByUserName(string Username);
        Task DeleteDS_HostInfo(DS_HostInfo dS_HostInfo);
        Task<string> PostHostRequest(string Hosts, string ServerType, string Name, string Password, string Region, int MNP, string PG, string IG);
        Task<List<Server>> GetServersByServerType(string ServerType);
        Task<List<LFG>> GetLFGs();
        Task AddLFG(LFG lFG);
        Task<List<PlayUser>> GetPlayUsers();
        //Task<bool> IsEmailConfirmed(SaveUser saveUser);
        //Task<string> Login(string userName, string password, string preDevice, int isPlatform);

        //Task<string> CharSelected(string userName, string currentChar);

        //Task<List<Server>> GetSpecificServerList(bool isHosted, string name);
        //Task<string> PlayerJoined(string ip, string port);
        Task<string> GetIP();
        
        //Task<string> PlayerLeft(string ip, string port);
    }
}
