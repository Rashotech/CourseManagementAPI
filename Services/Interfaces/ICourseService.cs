using CourseManagement.Database.Models;
using CourseManagement.Models.Dto.Course;

namespace CourseManagement.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(CourseRequestDto courseRequestDto, string InstructorId);
        Task EditCourseAsync(Guid CourseId, string InstructorId, CourseRequestDto courseRequestDto);
        Task DeleteCourseAsync(Guid CourseId, string InstructorId);
        Task GetCoursesByInstructorAsync(string InstructorId);
        Task GetAllCoursesAsync();
        Task<Course> GetSingleCourseByIdAsync(Guid CourseId);
    }
}