using System.ComponentModel.DataAnnotations;

namespace PhotoBank.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
