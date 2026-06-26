using HomeCare.Models;

namespace HomeCare.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Service> FeaturedServices { get; set; } = new();

        public List<ServiceCategory> Categories { get; set; } = new();

        public int TotalServices { get; set; }

        public int TotalCategories { get; set; }

        public int CompletedBookings { get; set; }

        public int TotalReviews { get; set; }
    }
}
