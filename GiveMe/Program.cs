using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiveMe.Data;
using GiveMe.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GiveMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {

            var scope = host.Services.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<IRepository>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            ctx.Database.EnsureCreated();

            var developerRole = new IdentityRole("developer");
            var adminRole = new IdentityRole("admin");
            var userRole = new IdentityRole("user");
            if (!ctx.Roles.Any())
            {
                roleMgr.CreateAsync(developerRole).GetAwaiter().GetResult();
                roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                roleMgr.CreateAsync(userRole).GetAwaiter().GetResult();
            }
            if (!ctx.Users.Any(u => u.UserName == "developer"))
            {
                var developerUser = new ApplicationUser
                {
                    UserName = "developer@developer.com",
                    Email = "developer@developer.com",
                    FirstName = "Alex",
                    LastName = "Koval",
                    EmailConfirmed = true
                };
                userMgr.CreateAsync(developerUser, "ufkjif").GetAwaiter().GetResult();
                userMgr.AddToRoleAsync(developerUser, developerRole.Name).GetAwaiter().GetResult();
            }
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
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
