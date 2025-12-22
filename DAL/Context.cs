using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;


namespace DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
       
        public DbSet<CV> CVs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany(u => u.SentMessages)
        .HasForeignKey(m => m.SenderId)
        .HasPrincipalKey(u => u.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
        new User
        {
            UserId = 1,
            Email = "anna@test.se",
            PasswordHash = "TESTHASH",
            IsActive = true
        },
        new User
        {
            UserId = 2,
            Email = "erik@test.se",
            PasswordHash = "TESTHASH",
            IsActive = true
        },
        



        new User
        {
            UserId = 3,
            Email = "anna2@test.se",
            PasswordHash = "TESTHASH",
            IsActive = true
        }
    );

            // PROFILES
            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    ProfileId = 1,
                    UserId = 1,
                    FullName = "Anna Andersson",
                    Bio = "Systemutvecklare .NET",
                    PictureUrl = "/images/anna.jpg",
                    IsPrivate = false
                },
                new Profile
                {
                    ProfileId = 2,
                    UserId = 2,
                    FullName = "Erik Eriksson",
                    Bio = "Backend-utvecklare",
                    PictureUrl = "/images/erik.jpg",
                    IsPrivate = true
                },

                new Profile
                {
                    ProfileId = 3,
                    UserId = 3,
                    FullName = "Anna Andersson",
                    Bio = "Junior .NET-utvecklare",
                    PictureUrl = "/images/anna2.jpg",
                    IsPrivate = false
                }
            );

            // CVs
            modelBuilder.Entity<CV>().HasData(
                new CV
                {
                    CVId = 1,
                    ProfileId = 1,
                    Education = "Systemvetenskap, ORU",
                    Experience = "2 år som .NET-utvecklare"
                },
                new CV
                {
                    CVId = 2,
                    ProfileId = 2,
                    Education = "Datavetenskap",
                    Experience = "Backend-utvecklare på startup"
                },

                new CV
                {
                    CVId = 3,
                    ProfileId = 3,
                    Education = "YH-utbildning i .NET",
                    Experience = "Praktik på IT-företag"
                }
            );

            // PROJECTS
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    ProjectId = 1,
                    Title = "CV-siten",
                    Description = "ASP.NET MVC-projekt"
                },
                new Project
                {
                    ProjectId = 2,
                    Title = "Portfolio",
                    Description = "Webbplats för att visa projekt"
                });
        }
    }
}
