using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HomeCare.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@homecare.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Address = "Admin Address",
                    LastLogin = DateTime.Now
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}