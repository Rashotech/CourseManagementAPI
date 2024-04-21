namespace CourseManagement.Models.Dto.Course
{
    public class CourseReponseDto: CourseDto
    {
        public required string Id { get; set; }
        public required UserDto Instructor { get; set; }
    }
}