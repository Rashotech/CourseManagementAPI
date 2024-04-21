using CourseManagement.Database.Models;
using CourseManagement.Models.Dto.Course;

namespace CourseManagement.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Guid> AddCourseAsync(CourseRequestDto courseRequestDto, string InstructorId);
        Task EditCourseAsync(Guid CourseId, string InstructorId, CourseRequestDto courseRequestDto);
        Task DeleteCourseAsync(Guid CourseId, string InstructorId);
        Task<List<CourseReponseDto>> GetCoursesByInstructorAsync(string InstructorId);
        Task<List<CourseReponseDto>> GetAllCoursesAsync();
        Task<CourseReponseDto> GetSingleCourseByIdAsync(Guid CourseId);
    }
}