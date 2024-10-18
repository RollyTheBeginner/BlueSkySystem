using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class CashAdvanceDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Middle Name is required.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Date Required")]
        public DateTime DateRequired { get; set; }


        [Required(ErrorMessage = "Expense Request Purpose/Description is required.")]
        [Display(Name = "Expense Request Purpose/Description")]
        public string Purpose { get; set; }


        [Required(ErrorMessage = "Amount is required.")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "Electronic Signature (Requester)")]
        public IFormFile? ImageFile1 { get; set; }

        [Display(Name = "Cover Letter (Requester)")]
        public IFormFile? CoverLetter { get; set; }

        /*
        [Display(Name = "Electronic Signature (Recommending Approval)")]
        public IFormFile? ImageFile2 { get; set; }

        [Display(Name = "Electronic Signature (Approved by:)")]
        public IFormFile? ImageFile3 { get; set; }

        [Display(Name = "Amount Received by:")]
        public string? AmountReceivedby { get; set; }
        */
    }
}
