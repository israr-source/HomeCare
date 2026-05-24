using System.ComponentModel.DataAnnotations;

namespace HomeCare.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        // FOREIGN KEY
        [Display(Name = "Category")]
        public int ServiceCategoryId { get; set; }

        // NAVIGATION PROPERTY
        public ServiceCategory? ServiceCategory { get; set; }
    }
}