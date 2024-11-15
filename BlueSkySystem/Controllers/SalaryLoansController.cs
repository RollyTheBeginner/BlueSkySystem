using BlueSkySystem.Data;
using BlueSkySystem.Models.SalaryLoan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using BlueSkySystem.Models;

namespace BlueSkySystem.Controllers
{
    [Authorize]
    public class SalaryLoansController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public SalaryLoansController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult Index()
        {
            var salaryLoans = context.SalaryLoans
                .Include(sl => sl.SalaryLoanStatus)
                .OrderByDescending(sl => sl.Id)
                .ToList();
            return View(salaryLoans);
        }

        public IActionResult AwaitingApproval()
        {
            var cashadvances = context.SalaryLoans
                .Include(ca => ca.SalaryLoanStatus)
                .Where(ca => ca.SalaryLoanStatus.Name == "Awaiting Approval")
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }

        public IActionResult PendingListView()
        {
            var cashadvances = context.SalaryLoans
                .Include(ca => ca.SalaryLoanStatus)
                .Where(ca => ca.SalaryLoanStatus.Name == "Pending Status")
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult ApprovedList()
        {
            var cashadvances = context.SalaryLoans
                .Include(ca => ca.SalaryLoanStatus) // Include related status
                .Where(ca => ca.SalaryLoanStatus.Name == "Approved") // Filter by approved status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult RejectedList()
        {
            var cashadvances = context.SalaryLoans
                .Include(ca => ca.SalaryLoanStatus) // Include related status
                .Where(ca => ca.SalaryLoanStatus.Name == "Rejected") // Filter by rejected status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryLoans = await context.SalaryLoans.FirstOrDefaultAsync(sl => sl.Id == id);

            if (salaryLoans == null)
            {
                return NotFound();
            }
            return View(salaryLoans);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalaryLoanDto salaryLoanDto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await context.Users.FindAsync(userId);

            var awaitingApproval = await context.SalaryLoanStatuses.FirstOrDefaultAsync(x => x.Name == "Awaiting Approval");

            if (awaitingApproval != null)
            {
                salaryLoanDto.SalaryLoanStatusId = awaitingApproval.Id;
            }
            else
            {
                ModelState.AddModelError("Status", "Awaiting Approval does not exist.");
            }

            // Check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            // Validate image file
            if (salaryLoanDto.ImageFile1 == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            // Validate cover letter file
            if (salaryLoanDto.CoverLetter == null)
            {
                ModelState.AddModelError("CoverLetter", "The cover letter is required");
            }

            // If there are validation errors, return the form with errors
            if (!ModelState.IsValid)
            {
                return View(salaryLoanDto);
            }

            // Save the image file with a unique name
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(salaryLoanDto.ImageFile1.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "ESignature", newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                salaryLoanDto.ImageFile1.CopyTo(stream);
            }

            // Save the cover letter with its original name
            string coverLetterOriginalFileName = salaryLoanDto.CoverLetter.FileName;
            string coverLetterFullPath = Path.Combine(environment.WebRootPath, "coverletter", coverLetterOriginalFileName);

            using (var stream = System.IO.File.Create(coverLetterFullPath))
            {
                salaryLoanDto.CoverLetter.CopyTo(stream);
            }

            // Create the new SalaryLoan object and map from DTO
            BlueSkySystem.Models.SalaryLoan.SalaryLoan newSalaryLoan = new BlueSkySystem.Models.SalaryLoan.SalaryLoan()
            {
                FirstName = salaryLoanDto.FirstName,
                MiddleName = salaryLoanDto.MiddleName,
                LastName = salaryLoanDto.LastName,
                Department = salaryLoanDto.Department,
                Address = salaryLoanDto.Address,
                Amount = salaryLoanDto.Amount,
                ImageFileName1 = newFileName,
                CreatedById = currentUser.FullName,
                CreatedOn = DateTime.Now,
                SalaryLoanStatusId = salaryLoanDto.SalaryLoanStatusId,
                CoverLetterName = coverLetterOriginalFileName,
            };

            //
            context.SalaryLoans.Add(newSalaryLoan);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", "SalaryLoans", new {id = newSalaryLoan.Id});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var salaryloans = context.SalaryLoans.Find(id);

            if (salaryloans == null)
            {
                return RedirectToAction("Index", "SalaryLoans");
            }

            var salaryLoanDto = new SalaryLoanDto()
            {
                FirstName = salaryloans.FirstName,
                MiddleName = salaryloans.MiddleName,
                LastName = salaryloans.LastName,
                Department = salaryloans.Department,
                Address = salaryloans.Address,
                Amount = salaryloans.Amount,
            };

            ViewData["CreatedById"] = salaryloans.CreatedById;
            ViewData["CreatedOn"] = salaryloans.CreatedOn;
            ViewData["SalaryLoanId"] = salaryloans.Id;
            ViewData["ImageFileName1"] = salaryloans.ImageFileName1;
            ViewData["ImageFileName2"] = salaryloans.ImageFileName2;
            ViewData["ImageFileName3"] = salaryloans.ImageFileName3;
            ViewData["CoverLetterName"] = salaryloans.CoverLetterName;
            ViewData["RecommendingApprovalId"] = salaryloans.RecommendingApprovalId;
            ViewData["ApprovedById"] = salaryloans.ApprovedById;

            return View(salaryLoanDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalaryLoanDto salaryLoanDto)
        {
            var salaryloans = context.SalaryLoans.Find(id);
            if (salaryloans == null)
            {
                return RedirectToAction("Index", "SalaryLoans");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ModifiedById"] = salaryloans.ModifiedById;
                ViewData["ModifiedOn"] = salaryloans.ModifiedOn;
                ViewData["SalaryLoanId"] = salaryloans.Id;
                ViewData["ImageFileName1"] = salaryloans.ImageFileName1;
                ViewData["ImageFileName2"] = salaryloans.ImageFileName2;
                ViewData["ImageFileName3"] = salaryloans.ImageFileName3;
                ViewData["CoverLetterName"] = salaryloans.CoverLetterName;
                ViewData["RecommendingApprovalId"] = salaryloans.RecommendingApprovalId;
                ViewData["ApprovedById"] = salaryloans.ApprovedById;

                return View(salaryLoanDto);
            }

            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the current user from the database
            var currentUser = await context.Users.FindAsync(userId);

            // Check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            // Save the image file with a unique name
            string newFileName2 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(salaryLoanDto.ImageFile2.FileName);
            string imageFullPath2 = Path.Combine(environment.WebRootPath, "ApproversESignature", newFileName2);

            using (var stream = System.IO.File.Create(imageFullPath2))
            {
                salaryLoanDto.ImageFile2.CopyTo(stream);
            }

            // Save the image file with a unique name
            string newFileName3 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(salaryLoanDto.ImageFile3.FileName);
            string imageFullPath3 = Path.Combine(environment.WebRootPath, "ApproversESignature", newFileName3);

            using (var stream3 = System.IO.File.Create(imageFullPath2))
            {
                salaryLoanDto.ImageFile2.CopyTo(stream3);
            }

            //update the image file if we have a new image file
            string newFileName = salaryloans.ImageFileName1;
            if (salaryLoanDto.ImageFile1 != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(salaryLoanDto.ImageFile1.FileName);

                string imageFullPath = environment.WebRootPath + "/ESignature/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    salaryLoanDto.ImageFile1.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/ESignature/" + salaryloans.ImageFileName1;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the cover letter file if we have a new cover letter file
            string coverLetterOriginalFileName = salaryloans.CoverLetterName;
            if (salaryLoanDto.CoverLetter != null)
            {
                coverLetterOriginalFileName = salaryLoanDto.CoverLetter.FileName;
                coverLetterOriginalFileName += Path.GetExtension(salaryLoanDto.CoverLetter.FileName);

                string coverLetterFullPath = environment.WebRootPath + "/coverletter/" + coverLetterOriginalFileName;
                using (var stream = System.IO.File.Create(coverLetterFullPath))
                {
                    salaryLoanDto.CoverLetter.CopyTo(stream);
                }

                // Delete the old cover letter if it exists
                string oldCoverLetterPath = Path.Combine(environment.WebRootPath, "/coverletter/", salaryloans.CoverLetterName);
                if (System.IO.File.Exists(oldCoverLetterPath))
                {
                    System.IO.File.Delete(oldCoverLetterPath);
                }
            }

            //update the product in the database
            salaryloans.FirstName = salaryLoanDto.FirstName;
            salaryloans.MiddleName = salaryLoanDto.MiddleName;
            salaryloans.LastName = salaryLoanDto.LastName;
            salaryloans.Department = salaryLoanDto.Department;
            salaryloans.DateSubmitted = salaryLoanDto.DateSubmitted;
            salaryloans.Address = salaryLoanDto.Address;
            salaryloans.Amount = salaryLoanDto.Amount;
            salaryloans.ImageFileName1 = newFileName;
            salaryloans.CoverLetterName = coverLetterOriginalFileName;
            salaryloans.ModifiedById = currentUser.FullName;
            salaryloans.ModifiedOn = DateTime.Now;
            salaryloans.ImageFileName2 = newFileName2;
            salaryloans.ImageFileName3 = newFileName3;

            context.SaveChanges();

            return RedirectToAction("Index", "SalaryLoans");
        }

        public IActionResult Delete(int id)
        {
            var salaryloans = context.SalaryLoans.Find(id);
            if (salaryloans == null)
            {
                return RedirectToAction("Index", "SalaryLoans");
            }

            string imageFullPath = environment.WebRootPath + "/ESignature/" + salaryloans.ImageFileName1;
            System.IO.File.Delete(imageFullPath);

            context.SalaryLoans.Remove(salaryloans);
            context.SaveChanges();
            return RedirectToAction("Index", "SalaryLoans");

        }
    }
}
