using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.Models
{
    public class Company : UserActivity
    {
        public int Id { get; set; } // Unique identifier for the company


        [Required(ErrorMessage = "Company Name is required.")]

        public string Name { get; set; } // Name of the company
        [StringLength(500)]
        public string Description { get; set; } // Description of the company
    }
}
