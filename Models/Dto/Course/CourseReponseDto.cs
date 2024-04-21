namespace CourseManagement.Models.Dto.Course
{
    public class CourseReponseDto: CourseDto
    {
        public required UserDto Instructor { get; set; }
    }
}