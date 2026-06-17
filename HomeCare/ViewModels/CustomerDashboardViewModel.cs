using HomeCare.Models;

namespace HomeCare.ViewModels
{
    public class CustomerDashboardViewModel
    {
        public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }

        public int CompletedBookings { get; set; }

        public int TotalReviews { get; set; }

        public List<Booking> RecentBookings { get; set; }
            = new();
    }
}