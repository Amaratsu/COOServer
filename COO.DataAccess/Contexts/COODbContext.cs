using COO.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace COO.DataAccess.Contexts
{
    public class COODbContext : DbContext
    {
        public COODbContext(
            DbContextOptions<COODbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Domain.Core.Server> Servers { get; set; }
    }
}
