namespace COO.Server.Features.MMO
{
    using COO.Server.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMMOService
    {
        Task<ActiveLogin> FindActiveLoginAsync(int userId);
        Task<int> CreateActiveLoginAsync(int userId, string sessionKey, int? characterId);
        Task<bool> DeleteActiveLoginAsync(int userId);
        Task<bool> UpdateActiveLoginAsync(int userId, string sessionKey, int? characterId);
        Task<Character> FindCharacterByCharacterIdAsync(int characterId);
        Task<Character> FindCharacterByCharacterIdAndUserIdAsync(int characterId, int userId);
        Task<Character> FindCharacterByNameAsync(string name);
        Task<int> CreateCharacterAsync(
            int userId, string name, int classId, int gender,
            int health, int mana, double posx, double posy, double posz,
            double yaw, string equipChest, string equipFeet,
            string equipHands, string equipHead, string equipLegs,
            string hotbar0, string hotbar1, string hotbar2, string hotbar3
            );
        Task<bool> UpdateCharacterAsync(
            int characterId, int health, int mana, 
            int experience, int level, double posx, double posy, double posz,
            double yaw, string equipChest, string equipFeet,
            string equipHands, string equipHead, string equipLegs,
            string hotbar0, string hotbar1, string hotbar2, string hotbar3
            );
        Task<bool> UpdateCharacterClanAsync(Character character, int clanId);
        Task<bool> UpdateCharactersClanAsync(int clanId);
        Task<bool> DeleteCharacterAsync(int characterId);
        Task<List<Inventory>> GetInventoryListByCharacterIdAsync(int characterId);
        Task<bool> DeleteRangeInventoryByCharacterIdAsync(int characterId);
        Task<bool> AddRangeInventoryAsync(List<Inventory> inventory);
        Task<List<Quest>> GetQuestListByCharacterIdAsync(int characterId);
        Task<bool> DeleteRangeQuestsByCharacterIdAsync(int characterId);
        Task<bool> AddRangeQuestsAsync(List<Quest> quests);
        Task<Clan> FindClanByIdAsync(int id);
        Task<Clan> FindClanByNameAsync(string name);
        Task<int> CreateClanAsync(int leaderId, string name);
        Task<bool> DeleteClanAsync(int clanId);
        Task<List<ClanCharacter>> GetClans();
        Task<List<Character>> GetCharacterListByUserIdAsync(int userId);
        Task<string> GetIPAsync();
        Task<string> GetServer();
    }
}
