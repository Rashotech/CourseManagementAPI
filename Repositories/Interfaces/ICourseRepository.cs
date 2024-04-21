using CourseManagement.Database.Models;

namespace CourseManagement.Repositories.Interfaces
{
    public interface ICourseRepository: IGenericRepository<Course>
    {
        Task<Course> GetSingleCourseByIdAsync(Guid accountId);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Course>> GetCoursesByInstructorAsync(string InstructorId);
    }
}