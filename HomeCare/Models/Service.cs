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
        [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        // FOREIGN KEY
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int ServiceCategoryId { get; set; }

        // NAVIGATION PROPERTY
        public ServiceCategory? ServiceCategory { get; set; }
    }
}
