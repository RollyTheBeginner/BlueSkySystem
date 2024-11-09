using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlueSkySystem.Models;
using BlueSkySystem.Models.SalaryLoan;

namespace BlueSkySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<CashAdvance> CashAdvances { get; set; }
        public DbSet<SalaryLoan> SalaryLoans { get; set; } // Changed to plural for consistency
        public DbSet<SalaryLoanStatus> SalaryLoanStatuses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CashAdvanceStatus> CashAdvanceStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial cash advance statuses
            modelBuilder.Entity<CashAdvanceStatus>().HasData(
                new CashAdvanceStatus { Id = 1, Name = "Awaiting Approval", Description = "Awaiting Approval." },
                new CashAdvanceStatus { Id = 2, Name = "Pending Status", Description = "Pending Status." },
                new CashAdvanceStatus { Id = 3, Name = "Approved", Description = "Approved for processing." },
                new CashAdvanceStatus { Id = 4, Name = "Rejected", Description = "Rejected." }
            );

            // Seed initial salary loan statuses
            modelBuilder.Entity<SalaryLoanStatus>().HasData(
                new SalaryLoanStatus { Id = 1, Name = "Awaiting Approval", Description = "Awaiting Approval." },
                new SalaryLoanStatus { Id = 2, Name = "Pending Status", Description = "Pending Status." },
                new SalaryLoanStatus { Id = 3, Name = "Approved", Description = "Approved for processing." },
                new SalaryLoanStatus { Id = 4, Name = "Rejected", Description = "Rejected." }
            ); 

            // Seed initial company data
            modelBuilder.Entity<Company>().HasData(
               new Company { Id = 1, Name = "BlueSky", Description = "Blue Sky Trading Inc., Co." },
               new Company { Id = 2, Name = "Swisspharma", Description = "Swisspharma Research Laboratories, Inc." }
           );// Closing parenthesis added here

        }
    }
}
