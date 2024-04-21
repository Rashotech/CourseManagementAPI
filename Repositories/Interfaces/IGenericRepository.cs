using System.Linq.Expressions;

namespace CourseManagement.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid Id);
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> Add(T entity);
        Task<bool> Delete(Guid Id); 
        void Update(T entity);
    }
}