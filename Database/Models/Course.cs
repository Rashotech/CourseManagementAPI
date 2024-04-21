using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Database.Models
{
    public class Course: BaseEntity
    {
        [Required]
        public required string CourseName { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string InstructorId { get; set; }
        
        [ForeignKey("InstructorId")]
        public ApplicationUser Instructor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}