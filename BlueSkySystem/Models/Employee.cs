using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class Employee : UserActivity
    {
        // Foreign key for Users
        [Required]
        public string UserId { get; set; }

        // Navigation property to Users class
        public Users User { get; set; }

        [Key]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Middle Name is required.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Home Address")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

    }

}
