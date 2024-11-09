using BlueSkySystem.Data;
using BlueSkySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace BlueSkySystem.Controllers
{
    [Authorize]
    public class CashAdvancesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public CashAdvancesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult Index()
        {
            var cashadvances = context.CashAdvances
       .Include(ca => ca.CashAdvanceStatus) // Include related status
       .OrderByDescending(ca => ca.Id)
       .ToList();
            return View(cashadvances);
        }


        // Awaiting Approval List
        public IActionResult AwaitingApproval()
        {
            return GetAwaitingCashAdvances();
        }

        public IActionResult AwaitingApprovalSwiss()
        {
            return GetAwaitingCashAdvances();
        }

        private IActionResult GetAwaitingCashAdvances()
        {
            var cashadvances = context.CashAdvances
                .Include(ca => ca.CashAdvanceStatus) // Include related status
                .Where(ca => ca.CashAdvanceStatus.Name == "Awaiting Approval") // Filter by approved status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }



        // Pending List
        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult PendingListView()
        {
            return GetPendingCashAdvances();
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult PendingListSwiss()
        {
            return GetPendingCashAdvances();
        }

        private IActionResult GetPendingCashAdvances()
        {
            var cashadvances = context.CashAdvances
                .Include(ca => ca.CashAdvanceStatus) // Include related status
                .Where(ca => ca.CashAdvanceStatus.Name == "Pending Status") // Filter by approved status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }


        // Approve List
        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult ApprovedList()
        {
            return GetApprovedCashAdvances();
        }

        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult ApprovedListSwiss() // Corrected from ApproveListSwiss to ApprovedListSwiss
        {
            return GetApprovedCashAdvances();
        }
        private IActionResult GetApprovedCashAdvances()
        {
            var cashadvances = context.CashAdvances
                .Include(ca => ca.CashAdvanceStatus) // Include related status
                .Where(ca => ca.CashAdvanceStatus.Name == "Approved") // Filter by approved status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }


        // Rejected List
        [Authorize(Roles = "Admin, Department Head")]
        public IActionResult RejectedList()
        {
            return GetRejectedCashAdvances();
        }

        public IActionResult RejectedListSwiss()
        {
            return GetRejectedCashAdvances();
        }

        private IActionResult GetRejectedCashAdvances()
        {
            var cashadvances = context.CashAdvances
                .Include(ca => ca.CashAdvanceStatus) // Include related status
                .Where(ca => ca.CashAdvanceStatus.Name == "Rejected") // Filter by approved status
                .OrderByDescending(ca => ca.Id)
                .ToList();

            return View(cashadvances);
        }

        // GET: CashAdvance/Details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashadvances = await context.CashAdvances.FirstOrDefaultAsync(ca => ca.Id == id);

            if (cashadvances == null)
            {
                return NotFound();
            }

            return View(cashadvances);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CashAdvanceDto cashadvanceDto)
        {

            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the current user from the database
            var currentUser = await context.Users.FindAsync(userId);

            var awaitingApproval = await context.CashAdvanceStatuses.FirstOrDefaultAsync(x => x.Name == "Awaiting Approval");

            if (awaitingApproval != null)
            {
                cashadvanceDto.CashAdvanceStatusId = awaitingApproval.Id;
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
            if (cashadvanceDto.ImageFile1 == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            // Validate cover letter file
            if (cashadvanceDto.CoverLetter == null)
            {
                ModelState.AddModelError("CoverLetter", "The cover letter is required");
            }

            // If there are validation errors, return the form with errors
            if (!ModelState.IsValid)
            {
                return View(cashadvanceDto);
            }


            // Save the image file with a unique name
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(cashadvanceDto.ImageFile1.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "ESignature", newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                cashadvanceDto.ImageFile1.CopyTo(stream);
            }

            // Save the cover letter with its original name
            string coverLetterOriginalFileName = cashadvanceDto.CoverLetter.FileName;
            string coverLetterFullPath = Path.Combine(environment.WebRootPath, "coverletter", coverLetterOriginalFileName);

            using (var stream = System.IO.File.Create(coverLetterFullPath))
            {
                cashadvanceDto.CoverLetter.CopyTo(stream);
            }

            // Create the new CashAdvance object and map from DTO
            CashAdvance newCashAdvance = new CashAdvance()
            {
                FirstName = cashadvanceDto.FirstName,
                MiddleName = cashadvanceDto.MiddleName,
                LastName = cashadvanceDto.LastName,
                Department = cashadvanceDto.Department,
                DateSubmitted = cashadvanceDto.DateSubmitted,
                DateRequired = cashadvanceDto.DateRequired,
                Purpose = cashadvanceDto.Purpose,
                Amount = cashadvanceDto.Amount,
                ImageFileName1 = newFileName,
                CreatedById = currentUser.FullName,  // Use FullName or FirstName as needed
                CreatedOn = DateTime.Now,
                CoverLetterName = coverLetterOriginalFileName, // Assuming you have this property
                AmountReceivedby = cashadvanceDto.AmountReceivedby,
                CashAdvanceStatusId = cashadvanceDto.CashAdvanceStatusId,
            };


            // Add to the context and save changes asynchronously
            context.CashAdvances.Add(newCashAdvance);
            await context.SaveChangesAsync();

            // Redirect to the details page of the newly created CashAdvance
            return RedirectToAction("Details", "CashAdvances", new { id = newCashAdvance.Id });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var cashadvances = context.CashAdvances.Find(id);

            if (cashadvances == null)
            {
                return RedirectToAction("Index", "CashAdvances");
            }

            //create cashadvancesDto from cashadvances
            var cashadvancesDto = new CashAdvanceDto()
            {
                FirstName = cashadvances.FirstName,
                MiddleName = cashadvances.MiddleName,
                LastName = cashadvances.LastName,
                Department = cashadvances.Department,
                DateSubmitted = cashadvances.DateSubmitted,
                DateRequired = cashadvances.DateRequired,
                Purpose = cashadvances.Purpose,
                Amount = cashadvances.Amount,
                AmountReceivedby = cashadvances.AmountReceivedby,

            };

            ViewData["CreatedById"] = cashadvances.CreatedById;
            ViewData["CreatedOn"] = cashadvances.CreatedOn;
            ViewData["CashAdvanceId"] = cashadvances.Id;
            ViewData["ImageFileName1"] = cashadvances.ImageFileName1;
            ViewData["ImageFileName2"] = cashadvances.ImageFileName2;
            ViewData["ImageFileName3"] = cashadvances.ImageFileName3;
            ViewData["CoverLetterName"] = cashadvances.CoverLetterName;
            ViewData["RecommendingApprovalId"] = cashadvances.RecommendingApprovalId;
            ViewData["ApprovedById"] = cashadvances.ApprovedById;

            return View(cashadvancesDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CashAdvanceDto cashadvanceDto)
        {
            var cashadvances = context.CashAdvances.Find(id);
            if (cashadvances == null)
            {
                return RedirectToAction("Index", "CashAdvances");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ModifiedById"] = cashadvances.ModifiedById;
                ViewData["ModifiedOn"] = cashadvances.ModifiedOn;
                ViewData["CashAdvanceId"] = cashadvances.Id;
                ViewData["ImageFileName1"] = cashadvances.ImageFileName1;
                ViewData["ImageFileName2"] = cashadvances.ImageFileName2;
                ViewData["ImageFileName3"] = cashadvances.ImageFileName3;
                ViewData["CoverLetterName"] = cashadvances.CoverLetterName;
                ViewData["RecommendingApprovalId"] = cashadvances.RecommendingApprovalId;
                ViewData["ApprovedById"] = cashadvances.ApprovedById;

                return View(cashadvanceDto);
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
            string newFileName2 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(cashadvanceDto.ImageFile2.FileName);
            string imageFullPath2 = Path.Combine(environment.WebRootPath, "ApproversESignature", newFileName2);

            using (var stream = System.IO.File.Create(imageFullPath2))
            {
                cashadvanceDto.ImageFile2.CopyTo(stream);
            }

            // Save the image file with a unique name
            string newFileName3 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(cashadvanceDto.ImageFile3.FileName);
            string imageFullPath3 = Path.Combine(environment.WebRootPath, "ApproversESignature", newFileName3);

            using (var stream3 = System.IO.File.Create(imageFullPath2))
            {
                cashadvanceDto.ImageFile2.CopyTo(stream3);
            }

            //update the image file if we have a new image file
            string newFileName = cashadvances.ImageFileName1;
            if (cashadvanceDto.ImageFile1 != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(cashadvanceDto.ImageFile1.FileName);

                string imageFullPath = environment.WebRootPath + "/ESignature/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    cashadvanceDto.ImageFile1.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/ESignature/" + cashadvances.ImageFileName1;
                System.IO.File.Delete(oldImageFullPath);
            }


            // update the cover letter file if we have a new cover letter file
            string coverLetterOriginalFileName = cashadvances.CoverLetterName;
            if (cashadvanceDto.CoverLetter != null)
            {
                coverLetterOriginalFileName = cashadvanceDto.CoverLetter.FileName;
                coverLetterOriginalFileName += Path.GetExtension(cashadvanceDto.CoverLetter.FileName);

                string coverLetterFullPath = environment.WebRootPath + "/coverletter/" + coverLetterOriginalFileName;
                using (var stream = System.IO.File.Create(coverLetterFullPath))
                {
                    cashadvanceDto.CoverLetter.CopyTo(stream);
                }

                // Delete the old cover letter if it exists
                string oldCoverLetterPath = Path.Combine(environment.WebRootPath, "/coverletter/", cashadvances.CoverLetterName);
                if (System.IO.File.Exists(oldCoverLetterPath))
                {
                    System.IO.File.Delete(oldCoverLetterPath);
                }
            }


            //update the product in the database
            cashadvances.FirstName = cashadvanceDto.FirstName;
            cashadvances.MiddleName = cashadvanceDto.MiddleName;
            cashadvances.LastName = cashadvanceDto.LastName;
            cashadvances.Department = cashadvanceDto.Department;
            cashadvances.DateSubmitted = cashadvanceDto.DateSubmitted;
            cashadvances.DateRequired = cashadvanceDto.DateRequired;
            cashadvances.Purpose = cashadvanceDto.Purpose;
            cashadvances.Amount = cashadvanceDto.Amount;
            cashadvances.ImageFileName1 = newFileName;
            cashadvances.CoverLetterName = coverLetterOriginalFileName;
            cashadvances.ModifiedById = currentUser.FullName;
            cashadvances.ModifiedOn = DateTime.Now;
            cashadvances.AmountReceivedby = cashadvanceDto.AmountReceivedby;
            cashadvances.ImageFileName2 = newFileName2;
            cashadvances.ImageFileName3 = newFileName3;

            context.SaveChanges();

            return RedirectToAction("Index", "CashAdvances");
        }

        public IActionResult Delete(int id)
        {
            var cashadvances = context.CashAdvances.Find(id);
            if (cashadvances == null)
            {
                return RedirectToAction("Index", "CashAdvances");
            }

            string imageFullPath = environment.WebRootPath + "/ESignature" + cashadvances.ImageFileName1;
            System.IO.File.Delete(imageFullPath);

            context.CashAdvances.Remove(cashadvances);
            context.SaveChanges();
            return RedirectToAction("Index", "CashAdvances");

        }

        public async Task<IActionResult> AwaitingApprovalCashAdvance(int id)
        {
            var cashAdvance = await context.CashAdvances.FindAsync(id);
            if (cashAdvance == null)
            {
                return NotFound();
            }

            // Get the current logged-in user's ID and name
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await context.Users.FindAsync(userId);

            //check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var awaitingApproval = await context.CashAdvanceStatuses.FirstOrDefaultAsync(s => s.Name == "Awaiting Approval");
            if (awaitingApproval != null)
            {
                cashAdvance.CashAdvanceStatusId = awaitingApproval.Id;

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CashAdvances");
        }

        public async Task<IActionResult> PendingStatusCashAdvance(int id)
        {
            var cashAdvance = await context.CashAdvances.FindAsync(id);
            if (cashAdvance == null)
            {
                return NotFound();
            }

            // Get the current logged-in user's ID and name
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await context.Users.FindAsync(userId);

            // Check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var pendingstatus = await context.CashAdvanceStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Status");
            if (pendingstatus != null)
            {
                cashAdvance.CashAdvanceStatusId = pendingstatus.Id;
                cashAdvance.RecommendingApprovalId = currentUser.FullName;
                cashAdvance.RecommendingApproveOn = DateTime.UtcNow;

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CashAdvances");
        }

        public async Task<IActionResult> ApproveCashAdvance(int id)
        {
            var cashAdvance = await context.CashAdvances.FindAsync(id);
            if (cashAdvance == null)
            {
                return NotFound();
            }

            // Get the current logged-in user's ID and name
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await context.Users.FindAsync(userId);

            // Check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var approvedStatus = await context.CashAdvanceStatuses.FirstOrDefaultAsync(s => s.Name == "Approved");
            if (approvedStatus != null)
            {
                cashAdvance.CashAdvanceStatusId = approvedStatus.Id;
                cashAdvance.ApprovedById = currentUser.FullName;
                cashAdvance.ApprovedOn = DateTime.UtcNow;

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CashAdvances");
        }

        public async Task<IActionResult> RejectCashAdvance(int id)
        {
            // Await for the async call
            var cashAdvance = await context.CashAdvances.FirstOrDefaultAsync(c => c.Id == id);
            if (cashAdvance == null)
            {
                return NotFound();
            }

            // Get the current logged-in user's ID and name
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await context.Users.FindAsync(userId);

            // Check if the user exists
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            // Await for the async call and check for null correctly
            var rejectStatus = await context.CashAdvanceStatuses.FirstOrDefaultAsync(s => s.Name == "Rejected");
            if (rejectStatus == null)
            {
                return BadRequest("Reject status not found.");
            }

            // Update status and save changes
            cashAdvance.CashAdvanceStatusId = rejectStatus.Id;

            cashAdvance.RejectedById = currentUser.FullName;
            cashAdvance.RejectedOn = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "CashAdvances");
        }



        public IActionResult DownloadCoverLetter(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return NotFound();
            }

            // Specify the path where your cover letters are stored
            string path = Path.Combine(environment.WebRootPath, "coverletter", filename);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            // Read the file bytes
            var fileBytes = System.IO.File.ReadAllBytes(path);

            // Determine the MIME type based on the file extension
            var contentType = "application/octet-stream"; // Fallback content type

            string extension = Path.GetExtension(filename).ToLowerInvariant();
            switch (extension)
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // for .docx
                    break;
                case ".xls":
                case ".xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // for .xlsx
                    break;
                case ".ppt":
                case ".pptx":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; // for .pptx
                    break;
                case ".txt":
                    contentType = "text/plain";
                    break;
                case ".csv":
                    contentType = "text/csv";
                    break;
                    // Add more cases as needed for different file types
            }

            return File(fileBytes, contentType, filename);
        }


        // Print Details
        [Authorize(Roles = "Admin, Department Head")]
        public async Task<IActionResult> PrintDetails(int id)
        {
            // Fetch the specific CashAdvance by id and include related status
            var cashadvance = await context.CashAdvances
                                   .Include(ca => ca.CashAdvanceStatus) // Include related status
                                   .FirstOrDefaultAsync(ca => ca.Id == id && ca.CashAdvanceStatus.Name == "Approved"); // Filter by id and approved status

            // Check if the cashadvance is null
            if (cashadvance == null)
            {
                return NotFound(); // Return a NotFound result if not found
            }

            return View(cashadvance); // Return the single cashadvance object to the view
        }

    }
}
