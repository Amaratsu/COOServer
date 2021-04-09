namespace COO.Server.Controllers.MMO
{
    public class MmoService
    {
        //private readonly COODbContext data;

        //public MMOService(COODbContext data) => this.data = data;

        //public async Task<string> GetIPAsync()
        //{
        //    var httpClient = new HttpClient();
        //    return await httpClient.GetStringAsync("https://api.ipify.org");
        //}

        //public async Task<bool> UpdateCharacterAsync(
        //    int characterId,
        //    int health, 
        //    int mana,
        //    int experience,
        //    int level,
        //    double posx, 
        //    double posy, 
        //    double posz, 
        //    double yaw, 
        //    string equipChest, 
        //    string equipFeet, 
        //    string equipHands, 
        //    string equipHead, 
        //    string equipLegs, 
        //    string hotbar0, 
        //    string hotbar1, 
        //    string hotbar2, 
        //    string hotbar3
        //    )
        //{
        //    var character = await FindCharacterByCharacterIdAsync(characterId);
        //    if (character != null)
        //    {
        //        character.Health = health;
        //        character.Mana = mana;
        //        character.Experience = experience;
        //        character.Level = level;
        //        character.PosX = posx;
        //        character.PosY = posy;
        //        character.PosZ = posz;
        //        character.RotationYaw = yaw;
        //        character.EquipChest = equipChest;
        //        character.EquipFeet = equipFeet;
        //        character.EquipHands = equipHands;
        //        character.EquipHead = equipHead;
        //        character.EquipLegs = equipLegs;
        //        character.Hotbar0 = hotbar0;
        //        character.Hotbar1 = hotbar1;
        //        character.Hotbar2 = hotbar2;
        //        character.Hotbar3 = hotbar3;

        //        await this.data.SaveChangesAsync();

        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<string> GetServerIP()
        //{
        //    var httpClient = new HttpClient();
        //    return await httpClient.GetStringAsync("https://api.ipify.org");
        //}

        //public async Task<bool> UpdateCharacterClanAsync(Character character, int clanId)
        //{
        //    character.ClanId = clanId;

        //    await this.data.SaveChangesAsync();

        //    return true;
        //}

        //public async Task<bool> UpdateCharactersClanAsync(int clanId)
        //{
        //    var characters = await this.data.Characters.Where(c => c.ClanId == clanId).ToListAsync();
        //    if (characters.Count > 0)
        //    {
        //        characters.ForEach(c => clanId = 0);
        //        await this.data.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<int> CreateClanAsync(int leaderId, string name)
        //{
        //    var clan = new Clan
        //    {
        //        LeaderId = leaderId,
        //        Name = name
        //    };

        //    this.data.Add(clan);

        //    await this.data.SaveChangesAsync();

        //    return clan.Id;
        //}

        //public async Task<bool> DeleteClanAsync(int clanId)
        //{
        //    var clan = await FindClanByIdAsync(clanId);
        //    if (clan != null)
        //    {
        //        this.data.Clans.Remove(clan);
        //        await this.data.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<List<ClanCharacter>> GetClans()
        //    => await this.data.Characters
        //        .Join(this.data.Clans,
        //            ch => ch.ClanId,
        //            c => c.Id,
        //            (ch, c) => new ClanCharacter
        //            {
        //                CharacterId = ch.Id,
        //                CharacterName = ch.Name,
        //                ClanId = c.Id,
        //                ClanName = c.Name,
        //                IsLeader = ch.Id == c.LeaderId
        //            }
        //        )
        //        .Where(c => c.ClanId != 0).
        //        ToListAsync();
    }
}
