using COO.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace COO.DataAccess.Contexts
{
    public class CooDbContext : DbContext
    {
        public CooDbContext(
            DbContextOptions<CooDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        //public DbSet<Character> Characters { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Alliance> Alliances { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<GameServer> InfoServers { get; set; }
        public DbSet<InitializeDataCharacter> InitializeDataCharacters { get; set; }
    }
}
