using Microsoft.AspNetCore.Identity;
using Sección_25___Identity__Authorization_and_Security.Models;

namespace Sección_25___Identity__Authorization_and_Security.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var config = services.GetRequiredService<IConfiguration>();

            foreach (var roleName in new[] { "Admin", "User" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            string? email = config["SeedAdmin:Email"];
            string? password = config["SeedAdmin:Password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return;

            if (await userManager.FindByEmailAsync(email) != null)
                return;

            var admin = new ApplicationUser { UserName = email, Email = email, DisplayName = "Administrator" };
            var result = await userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
