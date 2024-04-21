using CourseManagement.Database.Models;
using CourseManagement.Models.Dto.Course;

namespace CourseManagement.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(AddCourseRequestDto addCourseRequestDto, string InstructorId);
        Task EditCourseAsync(Guid CourseId, string InstructorId, EditCourseRequestDto editCourseRequestDto);
        Task DeleteCourseAsync(Guid CourseId, string InstructorId);
        Task GetCoursesByInstructorAsync(string InstructorId);
        Task<Course> GetSingleCourseByIdAsync(Guid CourseId);
    }
}