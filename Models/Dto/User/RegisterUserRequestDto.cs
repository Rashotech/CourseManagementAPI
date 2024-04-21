using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models.Dto
{
    public class RegisterUserRequestDto
    {
        [Required]
        [MinLength(6)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.")]
        public required string Password { get; set; }
    }
}