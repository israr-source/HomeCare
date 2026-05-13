using Microsoft.AspNetCore.Identity;

namespace HomeCare.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string? RoleDescription { get; set; }
    }
}