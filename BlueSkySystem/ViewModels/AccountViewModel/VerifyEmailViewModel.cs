using System.ComponentModel.DataAnnotations;

namespace BlueSkySystem.ViewModels.AccountViewModel
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
