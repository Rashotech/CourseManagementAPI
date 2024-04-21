using System.ComponentModel.DataAnnotations;


namespace CourseManagement.Models.Dto.Course
{
    public class AddCourseRequestDto
    {
        [Required]
        public required string CourseName { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}