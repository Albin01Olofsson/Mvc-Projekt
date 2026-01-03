using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;


namespace DAL
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Profile> Profile { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // VIKTIGT – krävs för Identity
            base.OnModelCreating(modelBuilder);

            // MESSAGE → USER (Sender)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .HasPrincipalKey(u => u.Id)   // ✅ IdentityUser.Id
                .OnDelete(DeleteBehavior.Restrict);

            // MESSAGE → USER (Receiver)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .HasPrincipalKey(u => u.Id)   // ✅ IdentityUser.Id
                .OnDelete(DeleteBehavior.Restrict);

            // PROFILE ↔ USER (One-to-One)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PROFILE ↔ CV (One-to-One)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.CV)
                .WithOne(c => c.Profile)
                .HasForeignKey<CV>(c => c.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // PROJECTOWNER → USER
            modelBuilder.Entity<Project>()
              .HasOne(p => p.Owner)
              .WithMany()
              .HasForeignKey(p => p.OwnerId)
              .OnDelete(DeleteBehavior.Restrict);




        }
    }
}
