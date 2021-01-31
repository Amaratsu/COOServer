namespace COO.Server.Features.MMO
{
    using COO.Server.Data;
    using COO.Server.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MMOService : IMMOService
    {
        private readonly COODbContext data;

        public MMOService(COODbContext data) => this.data = data;

        public async Task<bool> CreateActiveLoginAsync(int userId, string sessionKey, int? characterId)
        {
            var activeLogin = new ActiveLogin
            {
                UserId = userId,
                SessionKey = sessionKey,
                CharacterId = characterId
            };

            this.data.Add(activeLogin);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateCharacterAsync(
            int userId, 
            string name, 
            int classId, 
            int gender, 
            int health, 
            int mana, 
            int posx, 
            int posy, 
            int posz, 
            decimal yaw,
            string equipChest,
            string equipFeet,
            string equipHands,
            string equipHead,
            string equipLegs,
            string hotbar0,
            string hotbar1,
            string hotbar2,
            string hotbar3)
        {
            var character = new Character
            {
                UserId = userId,
                Name = name,
                Class = classId,
                Gender = gender,
                Health = health,
                Mana = mana,
                Level = 1,
                Experience = 0,
                Clan = 0,
                PosX = posx,
                PosY = posy,
                PosZ = posz,
                RotationYaw = yaw,
                EquipChest = equipChest,
                EquipFeet = equipFeet,
                EquipHands = equipHands,
                EquipHead = equipHead,
                EquipLegs = equipLegs,
                Hotbar0 = hotbar0,
                Hotbar1 = hotbar1,
                Hotbar2 = hotbar2,
                Hotbar3 = hotbar3
            };

            this.data.Add(character);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteActiveLoginAsync(int userId)
        {
            var activeLogin = await FindActiveLoginAsync(userId);
            if (activeLogin != null)
            {
                this.data.ActiveLogins.Remove(activeLogin);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCharacterAsync(int charId)
        {
            var character = await FindCharacterByCharIdAsync(charId);
            if (character != null)
            {
                this.data.Characters.Remove(character);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ActiveLogin> FindActiveLoginAsync(int userId)
            => await this.data
                .ActiveLogins
                .FirstOrDefaultAsync(al => al.UserId == userId);

        public async Task<Character> FindCharacterByCharIdAsync(int charId)
            => await this.data
                .Characters
                .FirstOrDefaultAsync(c => c.Id == charId);

        public async Task<Character> FindCharacterByCharIdAndUserIdAsync(int charId, int userId)
            => await this.data
                .Characters
                .FirstOrDefaultAsync(c => c.Id == charId && c.UserId == userId);

        public async Task<Character> FindCharacterByNameAsync(string name)
            => await this.data
                .Characters
                .FirstOrDefaultAsync(c => c.Name == name);

        public async Task<bool> UpdateActiveLoginAsync(int userId, string sessionKey, int? characterId)
        {
            var activeLogin = await FindActiveLoginAsync(userId);
            if (activeLogin != null)
            {
                activeLogin.SessionKey = sessionKey != null ? sessionKey : activeLogin.SessionKey;
                activeLogin.CharacterId = characterId != null ? characterId : activeLogin.CharacterId;

                await this.data.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<List<Inventory>> GetInventoryListByCharIdAsync(int charId)
            => await this.data
                .Inventories
                .Where(i => i.CharacterId == charId)
                .ToListAsync();

        public async Task<List<Quest>> GetQuestListByCharIdAsync(int charId)
            => await this.data
                .Quests
                .Where(q => q.CharacterId == charId)
                .ToListAsync();

        public async Task<Clan> FindClanAsync(int id)
            => await this.data
                .Clans
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Character>> GetCharacterListByUserIdAsync(int userId)
            => await this.data
                .Characters
                .Where(ch => ch.UserId == userId)
                .ToListAsync();
    }
}
