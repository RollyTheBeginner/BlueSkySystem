using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class Users : IdentityUser
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

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                // Concatenate the names and handle nulls
                return $"{FirstName?.Trim()} {(MiddleName?.Trim() + " ") ?? ""}{LastName?.Trim()}".Trim();
            }
        }
    }
}
