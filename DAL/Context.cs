using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


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





        }
    }
}
