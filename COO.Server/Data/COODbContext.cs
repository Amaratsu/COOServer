namespace COO.Server.Data
{
    //using System;
    //using System.Linq;
    //using System.Threading;
    //using System.Threading.Tasks;
    //using Infrastructure.Services;
    //using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    //using Models.Base;

    public class COODbContext : DbContext
    {
        //private readonly ICurrentUserService currentUser;

        public COODbContext(
            DbContextOptions<COODbContext> options)
            : base(options) { }

        //public DbSet<Cat> Cats { get; set; }

        //public DbSet<Profile> Profiles { get; set; }

        //public DbSet<Follow> Follows { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DbSet<DS_CreationRequest> DS_CreationRequests { get; set; }

        public DbSet<DS_HostInfo> DS_HostInfos { get; set; }

        public DbSet<DS_LoginRequest> DS_LoginRequests { get; set; }

        public DbSet<LFG> LFGs { get; set; }

        public DbSet<SaveUser> SaveUsers { get; set; }
        public DbSet<PlayUser> PlayUsers { get; set; }
        public DbSet<CharUser> CharUsers { get; set; }

        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    //this.ApplyAuditInformation();

        //    return base.SaveChanges(acceptAllChangesOnSuccess);
        //}

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        //{
        //    //this.ApplyAuditInformation();

        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

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

        //private void ApplyAuditInformation() 
        //    => this.ChangeTracker
        //        .Entries()
        //        .ToList()
        //        .ForEach(entry =>
        //        {
        //            var userName = this.currentUser.GetUserName();

        //            if (entry.Entity is IDeletableEntity deletableEntity)
        //            {
        //                if (entry.State == EntityState.Deleted)
        //                {
        //                    deletableEntity.DeletedOn = DateTime.UtcNow;
        //                    deletableEntity.DeletedBy = userName;
        //                    deletableEntity.IsDeleted = true;

        //                    entry.State = EntityState.Modified;

        //                    return;
        //                }
        //            }
                    
        //            if (entry.Entity is IEntity entity)
        //            {
        //                if (entry.State == EntityState.Added)
        //                {
        //                    entity.CreatedOn = DateTime.UtcNow;
        //                    entity.CreatedBy = userName;
        //                }
        //                else if (entry.State == EntityState.Modified)
        //                {
        //                    entity.ModifiedOn = DateTime.UtcNow;
        //                    entity.ModifiedBy = userName;
        //                }
        //            }
        //        });
    }
}
