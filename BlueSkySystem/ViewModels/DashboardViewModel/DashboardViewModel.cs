using BlueSkySystem.Models;

namespace BlueSkySystem.ViewModels.DashboardViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalCashAdvances { get; set; }
        public int AwaitingApprovalCount { get; set; }
        public int PendingStatusCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        public List<CashAdvance> RecentCashAdvances { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Users> Users { get; set; }
    }
}
