using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models.Dto
{
    public class LoginUserRequestDto
    {
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }
}