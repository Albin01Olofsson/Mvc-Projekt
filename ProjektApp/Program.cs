using Microsoft.EntityFrameworkCore;
using ProjektApp.Models;

namespace ProjektApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //
            // 1. MVC
            //
            builder.Services.AddControllersWithViews();

            //
            // 2. DbContext + ConnectionString
            //
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            var app = builder.Build();

            //
            // 3. Middleware pipeline
            //
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // (Auth kommer här senare)
            // app.UseAuthentication();
            app.UseAuthorization();

            //
            // 4. Routing
            //
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
