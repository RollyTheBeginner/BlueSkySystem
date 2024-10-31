using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models.SalaryLoan
{
    public class SalaryLoan : ApprovalActivity
    {
        [Display(Name = "Voucher No.")]
        public int Id { get; set; }

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
        public string? ImageFileName1 { get; set; }

        [Display(Name = "Cover Letter (Requester)")]
        public string? CoverLetterName { get; set; }

        public int? SalaryLoanStatusId { get; set; }

        public SalaryLoanStatus? SalaryLoanStatus { get; set; }

        [Display(Name = "Electronic Signature (Processed by:)")]
        public string? ImageFileName2 { get; set; }


        [Display(Name = "Electronic Signature (Approved by:)")]
        public string? ImageFileName3 { get; set; }
    }
}
