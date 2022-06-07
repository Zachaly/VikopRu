using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VikopRu.Data;
using VikopRu.Models;

namespace VikopRu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var scope = host.Services.CreateScope();

            try
            {
                // creates default role for users
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var userRole = new IdentityRole("User");
                var moderationRole = new IdentityRole("Moderator");
                var adminRole = new IdentityRole("Admin");

                if (!context.Roles.Any())
                {
                    roleManager.CreateAsync(userRole).Wait();
                    roleManager.CreateAsync(moderationRole).Wait();
                    roleManager.CreateAsync(adminRole).Wait();
                }
                else if (!context.Roles.Any(role => role.Name == userRole.Name))
                    roleManager.CreateAsync(userRole).Wait();
                else if (!context.Roles.Any(role => role.Name == moderationRole.Name))
                    roleManager.CreateAsync(moderationRole).Wait();
                else if (context.Roles.Any(role => role.Name == adminRole.Name))
                    roleManager.CreateAsync(adminRole).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"dupa {e.Message}");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
