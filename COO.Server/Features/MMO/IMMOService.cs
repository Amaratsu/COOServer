namespace COO.Server.Features.MMO
{
    using COO.Server.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMMOService
    {
        Task<ActiveLogin> FindActiveLoginAsync(int userId);
        Task<bool> CreateActiveLoginAsync(int userId, string sessionKey, int? characterId);
        Task<bool> DeleteActiveLoginAsync(int userId);
        Task<bool> UpdateActiveLoginAsync(int userId, string sessionKey, int? characterId);
        Task<Character> FindCharacterByCharIdAsync(int charId);
        Task<Character> FindCharacterByCharIdAndUserIdAsync(int charId, int userId);
        Task<Character> FindCharacterByNameAsync(string name);
        Task<bool> CreateCharacterAsync(
            int userId, string name, int classId, int gender,
            int health, int mana, int posx, int posy, int posz,
            decimal yaw, string equipChest, string equipFeet,
            string equipHands, string equipHead, string equipLegs,
            string hotbar0, string hotbar1, string hotbar2, string hotbar3
            );
        Task<bool> UpdateCharacterAsync(
            int charId, int health, int mana, 
            int experience, int level, int posx, int posy, int posz,
            decimal yaw, string equipChest, string equipFeet,
            string equipHands, string equipHead, string equipLegs,
            string hotbar0, string hotbar1, string hotbar2, string hotbar3
            );
        Task<bool> DeleteCharacterAsync(int charId);
        Task<List<Inventory>> GetInventoryListByCharIdAsync(int charId);
        Task<bool> DeleteRangeInventoryByCharIdAsync(int charId);
        Task<bool> AddRangeInventoryAsync(List<Inventory> inventory);
        Task<List<Quest>> GetQuestListByCharIdAsync(int charId);
        Task<bool> DeleteRangeQuestsByCharIdAsync(int charId);
        Task<bool> AddRangeQuestsAsync(List<Quest> quests);
        Task<Clan> FindClanAsync(int id);
        Task<List<Character>> GetCharacterListByUserIdAsync(int userId);
        Task<string> GetIPAsync();
        Task<string> GetServer();
    }
}
