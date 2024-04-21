using CourseManagement.Database;
using CourseManagement.Database.Models;
using CourseManagement.Exceptions;
using CourseManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repositories
{
    public class CourseRepository: GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Course> GetSingleCourseByIdAsync(Guid accountId)
        {
            return await _dbContext.Courses.Where(a => a.Id == accountId).Include("Instructor").FirstOrDefaultAsync() ?? 
                throw new NotFoundException("Course not found.");
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _dbContext.Courses.Include("Instructor").ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByInstructorAsync(string InstructorId)
        {
            return await _dbContext.Courses.Where(a => a.InstructorId == InstructorId).Include("Instructor").ToListAsync();
        }
    }
}