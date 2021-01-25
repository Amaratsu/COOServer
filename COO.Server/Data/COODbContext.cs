namespace COO.Server.Data
{
    using System.Threading;
    using System.Threading.Tasks;
    using COO.Server.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class COODbContext : DbContext
    {
        //private readonly ICurrentUserService currentUser;

        public COODbContext(
            DbContextOptions<COODbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActiveLogin> ActiveLogins { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Quest> Quests { get; set; }

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //this.ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            //this.ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder
        //        .Entity<Cat>()
        //        .HasQueryFilter(c => !c.IsDeleted)
        //        .HasOne(c => c.User)
        //        .WithMany(u => u.Cats)
        //        .HasForeignKey(c => c.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder
        //        .Entity<User>()
        //        .HasOne(u => u.Profile)
        //        .WithOne()
        //        .HasForeignKey<Profile>(p => p.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder
        //        .Entity<Follow>()
        //        .HasOne(f => f.User)
        //        .WithMany()
        //        .HasForeignKey(f => f.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder
        //        .Entity<Follow>()
        //        .HasOne(f => f.Follower)
        //        .WithMany()
        //        .HasForeignKey(f => f.FollowerId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    base.OnModelCreating(builder);
        //}
    }
}
