using System.ComponentModel.DataAnnotations;

namespace BlogProject.MVC.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be between 3 and 14 characters")]
        [MaxLength(14, ErrorMessage = "Username must be between 3 and 14 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password do not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
