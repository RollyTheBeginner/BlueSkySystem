using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class UserActivity
    {
        [Display(Name = "Created by:")]
        public string? CreatedById { get; set; }


        [Display(Name = "Created on:")]
        [DataType(DataType.Date)]
        public DateTime? CreatedOn { get; set; }


        [Display(Name = "Modified by:")]
        public string? ModifiedById { get; set; }


        [Display(Name = "Modified on:")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedOn { get; set; }
    }

    public class ApprovalActivity : UserActivity
    {
        [Display(Name = "Recommending Approval:")]
        public string? RecommendingApprovalId { get; set; }

        [Display(Name = "Approved on:")]
        [DataType(DataType.Date)]
        public DateTime? RecommendingApproveOn { get; set; }

        [Display(Name = "Rejected by:")]
        public string? RejectedById { get; set; }

        [Display(Name = "Rejected on:")]
        [DataType(DataType.Date)]
        public DateTime? RejectedOn { get; set; }


        [Display(Name = "Approved by:")]
        public string? ApprovedById { get; set; }

        [Display(Name = "Approved on:")]
        [DataType(DataType.Date)]
        public DateTime? ApprovedOn { get; set; }

        [Display(Name = "Amount Received by:")]
        public string? AmountReceivedby { get; set; }
    }

}
