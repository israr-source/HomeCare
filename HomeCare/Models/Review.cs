using HomeCare.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeCare.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        public string? CustomerId { get; set; }

        public ApplicationUser? Customer { get; set; }

        public string? ProviderId { get; set; }

        public ApplicationUser? Provider { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}