using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models.Dto.Course
{
    public class CourseDto
    {
        [Required(ErrorMessage = "Course name is required.")]
        public required string CourseName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        public required DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        public required DateTime EndDate { get; set; }
    }
}