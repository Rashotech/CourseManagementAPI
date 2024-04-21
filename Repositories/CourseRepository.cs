using CourseManagement.Database;
using CourseManagement.Database.Models;
using CourseManagement.Repositories.Interfaces;

namespace CourseManagement.Repositories
{
    public class CourseRepository: GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}