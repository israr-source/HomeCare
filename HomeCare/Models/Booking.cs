using HomeCare.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeCare.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // CUSTOMER
        public string? CustomerId { get; set; }

        public ApplicationUser? Customer { get; set; }

        // ASSIGNED PROVIDER

        public string? ProviderId { get; set; }

        public ApplicationUser? Provider { get; set; }

        // SERVICE
        [Display(Name = "Service")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a service.")]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }

        // BOOKING INFO
        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        public string? Notes { get; set; }

        // STATUS
        public string Status { get; set; } = "Pending";
    }
}
