using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlueSkySystem.Models;

namespace BlueSkySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<CashAdvance> CashAdvances { get; set; }

        public DbSet<CashAdvanceStatus> CashAdvanceStatuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Seed some initial statuses
            modelBuilder.Entity<CashAdvanceStatus>().HasData(
                new CashAdvanceStatus { Id = 1, Name = "Pending Status", Description = "Pending Status." },
                new CashAdvanceStatus { Id = 2, Name = "Approved", Description = "Approved for processing." },
                new CashAdvanceStatus { Id = 3, Name = "Rejected", Description = "Rejected." }
            );
        }
        
    }
}
