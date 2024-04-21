using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models.Dto.Course
{
    public class CourseDto
    {
       [Required]
        public required string CourseName { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateTime EndDate { get; set; }
    }
}