namespace CourseManagement.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICourseRepository Courses { get; }
        Task CommitAsync();
    }
}