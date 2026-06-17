namespace HomeCare.ViewModels
{
    public class ProviderDashboardViewModel
    {
        public int PendingJobs { get; set; }

        public int AcceptedJobs { get; set; }

        public int CompletedJobs { get; set; }

        public int TotalReviews { get; set; }

        public double AverageRating { get; set; }
    }
}