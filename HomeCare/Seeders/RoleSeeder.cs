using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HomeCare.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roles = { "Admin", "Customer", "Provider" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role,
                        RoleDescription = $"{role} role"
                    });
                }
            }
        }
    }
}