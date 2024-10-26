using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class CashAdvance : ApprovalActivity
    {
        [Key]
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
        public string? ImageFileName1 { get; set; }

        [Display(Name = "Cover Letter (Requester)")]
        public string? CoverLetterName { get; set; }

        [Display(Name = "Amount Received by:")]
        public string? AmountReceivedby { get; set; }


        public int? CashAdvanceStatusId { get; set; }

        public CashAdvanceStatus? CashAdvanceStatus { get; set; }


        [Display(Name = "Electronic Signature (Recommending Approval)")]
        public string? ImageFileName2 { get; set; }


        [Display(Name = "Electronic Signature (Approved by:)")]
        public string? ImageFileName3 { get; set; }


    }
}
