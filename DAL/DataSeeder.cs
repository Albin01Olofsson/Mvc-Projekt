using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public static class DataSeeder
{
    public static async Task SeedAsync(
        Context context,
        UserManager<User> userManager)
    {
        // Kör bara om databasen är tom
        if (context.Projects.Any())
            return;

        // =========================
        // USERS (Identity)
        // =========================
        var user1 = new User
        {
            UserName = "anna@mail.com",
            Email = "anna@mail.com",
            IsActive = true
        };

        var user2 = new User
        {
            UserName = "erik@mail.com",
            Email = "erik@mail.com",
            IsActive = true
        };

        await userManager.CreateAsync(user1, "Password123!");
        await userManager.CreateAsync(user2, "Password123!");

        // =========================
        // PROFILES
        // =========================
        var profile1 = new Profile
        {
            FullName = "Anna Andersson",
            Bio = "Junior .NET-utvecklare",
            PictureUrl = "/images/anna.jpg",
            IsPrivate = false,
            UserId = user1.Id
        };

        var profile2 = new Profile
        {
            FullName = "Erik Eriksson",
            Bio = "Backend-utvecklare",
            PictureUrl = "/images/erik.jpg",
            IsPrivate = false,
            UserId = user2.Id
        };

        context.Profile.AddRange(profile1, profile2);
        await context.SaveChangesAsync();

        // =========================
        // CVS
        // =========================
        context.CVs.AddRange(
            new CV
            {
                ProfileId = profile1.ProfileId,
                Education = "Systemvetenskap – Örebro universitet",
                Experience = "Praktik inom .NET"
            },
            new CV
            {
                ProfileId = profile2.ProfileId,
                Education = "Datavetenskap",
                Experience = "3 år backend-utveckling"
            }
        );

        // =========================
        // PROJECTS
        // =========================
        context.Projects.AddRange(
            new Project
            {
                Title = "CV-sidan",
                Description = "ASP.NET Core MVC-projekt"
            },
            new Project
            {
                Title = "Portfolio",
                Description = "Webbplats för att visa projekt"
            }
        );

        await context.SaveChangesAsync();
    }
}
