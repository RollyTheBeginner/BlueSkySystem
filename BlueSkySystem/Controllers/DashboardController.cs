using BlueSkySystem.Data;
using BlueSkySystem.ViewModels.DashboardViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueSkySystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext context;

        public DashboardController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Dashboard()
        {
            var viewModel = new DashboardViewModel
            {
                TotalUsers = context.Users.Count(),
                TotalEmployees = context.Employees.Count(),
                TotalCashAdvances = context.CashAdvances.Count(),
                AwaitingApprovalCount = context.CashAdvances.Count(c => c.CashAdvanceStatusId == 1), // Assuming 1 is the ID for 'Awaiting Approval'
                PendingStatusCount = context.CashAdvances.Count(c => c.CashAdvanceStatusId == 2), // Assuming 2 is the ID for 'Pending Status'
                ApprovedCount = context.CashAdvances.Count(c => c.CashAdvanceStatusId == 3), // Assuming 3 is the ID for 'Approved'
                RejectedCount = context.CashAdvances.Count(c => c.CashAdvanceStatusId == 4), // Assuming 4 is the ID for 'Rejected'
            };

            return View(viewModel);
        }
    }
}
