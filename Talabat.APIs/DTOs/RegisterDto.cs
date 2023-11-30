using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", 
            ErrorMessage = "Password must contain at least 1 uppercase letter," +
            " 1 lowercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; }
    }
}
