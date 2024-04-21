namespace CourseManagement.Models.Dto
{
    public class UserAuthResponseDto
    {
        public required UserDto User { get; set; }
        public required string Token { get; set; }
        public required string Role { get; set; }
    }
}