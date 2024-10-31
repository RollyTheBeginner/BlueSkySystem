using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models.SalaryLoan
{
    public class SalaryLoanDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Middle Name is required.")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Today;

        [Display(Name = "Electronic Signature (Borrower)")]
        public IFormFile? ImageFile1 { get; set; }

        [Display(Name = "Cover Letter (Requester)")]
        public IFormFile? CoverLetter { get; set; }

        public int? SalaryLoanStatusId { get; set; }

        public SalaryLoanStatus? SalaryLoanStatus { get; set; }

        [Display(Name = "Electronic Signature (Processed by:)")]
        public IFormFile? ImageFile2 { get; set; }


        [Display(Name = "Electronic Signature (Approved by:)")]
        public IFormFile? ImageFile3 { get; set; }
    }
}
