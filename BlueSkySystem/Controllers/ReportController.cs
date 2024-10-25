using BlueSkySystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlueSkySystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult GenerateProductReportPdf()
        {
            var products = _context.CashAdvances.ToList(); // Fetch products from the database

            // Placeholder for actual PDF generation logic
            using (MemoryStream stream = new MemoryStream())
            {
                // Example using iTextSharp or PdfSharp

                // Return PDF file
                return File(stream.ToArray(), "application/pdf", "ProductReport.pdf");
            }
        }

        public IActionResult GenerateProductReportWord()
        {
            var products = _context.CashAdvances.ToList(); // Fetch products from the database

            // Placeholder for actual Word document generation logic
            using (MemoryStream stream = new MemoryStream())
            {
                // Example using OpenXML

                // Return Word file
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ProductReport.docx");
            }
        }
    }
}
