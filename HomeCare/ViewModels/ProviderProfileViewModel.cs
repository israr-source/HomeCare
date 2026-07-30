using System.ComponentModel.DataAnnotations;

namespace HomeCare.ViewModels
{
    public class ProviderProfileViewModel
    {
        [Display(Name = "Email")]
        public string? Email { get; set; } // Read-only

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Service Category")]
        public string ServiceType { get; set; }
    }
}
