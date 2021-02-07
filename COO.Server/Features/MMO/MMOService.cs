namespace COO.Server.Features.MMO
{
    using COO.Server.Data;
    using COO.Server.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class MMOService : IMMOService
    {
        private readonly COODbContext data;

        public MMOService(COODbContext data) => this.data = data;

        public async Task<int> CreateActiveLoginAsync(int userId, string sessionKey, int? characterId)
        {
            var activeLogin = new ActiveLogin
            {
                UserId = userId,
                SessionKey = sessionKey,
                CharacterId = characterId
            };

            this.data.Add(activeLogin);

            await this.data.SaveChangesAsync();

            return activeLogin.Id;
        }

        public async Task<int> CreateCharacterAsync(
            int userId, 
            string name, 
            int classId, 
            int gender, 
            int health, 
            int mana, 
            double posx, 
            double posy, 
            double posz, 
            double yaw,
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
                ClanId = 0,
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

            return character.Id;
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

        public async Task<bool> DeleteCharacterAsync(int characterId)
        {
            var character = await FindCharacterByCharacterIdAsync(characterId);
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

        public async Task<Character> FindCharacterByCharacterIdAsync(int characterId)
            => await this.data
                .Characters
                .FirstOrDefaultAsync(c => c.Id == characterId);

        public async Task<Character> FindCharacterByCharacterIdAndUserIdAsync(int characterId, int userId)
            => await this.data
                .Characters
                .FirstOrDefaultAsync(c => c.Id == characterId && c.UserId == userId);

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

        public async Task<List<Inventory>> GetInventoryListByCharacterIdAsync(int characterId)
            => await this.data
                .Inventories
                .Where(i => i.CharacterId == characterId)
                .ToListAsync();

        public async Task<List<Quest>> GetQuestListByCharacterIdAsync(int characterId)
            => await this.data
                .Quests
                .Where(q => q.CharacterId == characterId)
                .ToListAsync();

        public async Task<Clan> FindClanByIdAsync(int id)
            => await this.data
                .Clans
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Clan> FindClanByNameAsync(string name)
            => await this.data
                .Clans
                .FirstOrDefaultAsync(c => c.Name == name);

        public async Task<List<Character>> GetCharacterListByUserIdAsync(int userId)
            => await this.data
                .Characters
                .Where(ch => ch.UserId == userId)
                .ToListAsync();

        public async Task<string> GetIPAsync()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api.ipify.org");
        }

        public async Task<bool> UpdateCharacterAsync(
            int characterId,
            int health, 
            int mana,
            int experience,
            int level,
            double posx, 
            double posy, 
            double posz, 
            double yaw, 
            string equipChest, 
            string equipFeet, 
            string equipHands, 
            string equipHead, 
            string equipLegs, 
            string hotbar0, 
            string hotbar1, 
            string hotbar2, 
            string hotbar3
            )
        {
            var character = await FindCharacterByCharacterIdAsync(characterId);
            if (character != null)
            {
                character.Health = health;
                character.Mana = mana;
                character.Experience = experience;
                character.Level = level;
                character.PosX = posx;
                character.PosY = posy;
                character.PosZ = posz;
                character.RotationYaw = yaw;
                character.EquipChest = equipChest;
                character.EquipFeet = equipFeet;
                character.EquipHands = equipHands;
                character.EquipHead = equipHead;
                character.EquipLegs = equipLegs;
                character.Hotbar0 = hotbar0;
                character.Hotbar1 = hotbar1;
                character.Hotbar2 = hotbar2;
                character.Hotbar3 = hotbar3;

                await this.data.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRangeInventoryByCharacterIdAsync(int characterId)
        {
            var oldInventory = await GetInventoryListByCharacterIdAsync(characterId);
            if (oldInventory.Count > 0)
            {
                this.data.Inventories.RemoveRange(oldInventory);

                await this.data.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRangeQuestsByCharacterIdAsync(int characterId)
        {
            var oldQuests = await GetQuestListByCharacterIdAsync(characterId);
            if (oldQuests.Count > 0)
            {
                this.data.Quests.RemoveRange(oldQuests);

                await this.data.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> AddRangeInventoryAsync(List<Inventory> inventory)
        {
            if (inventory.Count > 0)
            {
                this.data.Inventories.AddRange(inventory);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AddRangeQuestsAsync(List<Quest> quests)
        {
            if (quests.Count > 0)
            {
                this.data.Quests.AddRange(quests);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> GetServer()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api.ipify.org");
        }

        public async Task<bool> UpdateCharacterClanAsync(Character character, int clanId)
        {
            character.ClanId = clanId;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCharactersClanAsync(int clanId)
        {
            var characters = await this.data.Characters.Where(c => c.ClanId == clanId).ToListAsync();
            if (characters.Count > 0)
            {
                characters.ForEach(c => clanId = 0);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> CreateClanAsync(int leaderId, string name)
        {
            var clan = new Clan
            {
                LeaderId = leaderId,
                Name = name
            };

            this.data.Add(clan);

            await this.data.SaveChangesAsync();

            return clan.Id;
        }

        public async Task<bool> DeleteClanAsync(int clanId)
        {
            var clan = await FindClanByIdAsync(clanId);
            if (clan != null)
            {
                this.data.Clans.Remove(clan);
                await this.data.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ClanCharacter>> GetClans()
            => await this.data.Characters
                .Join(this.data.Clans,
                    ch => ch.ClanId,
                    c => c.Id,
                    (ch, c) => new ClanCharacter
                    {
                        CharacterId = ch.Id,
                        CharacterName = ch.Name,
                        ClanId = c.Id,
                        ClanName = c.Name,
                        IsLeader = ch.Id == c.LeaderId
                    }
                )
                .Where(c => c.ClanId != 0).
                ToListAsync();
    }
}
