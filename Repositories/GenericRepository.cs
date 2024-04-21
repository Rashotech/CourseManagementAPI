using System.Linq.Expressions;
using CourseManagement.Database;
using CourseManagement.Exceptions;
using CourseManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var entity = await _dbContext.Set<TEntity>().FindAsync(Id);
                if (entity != null)
                {
                    _dbContext.Set<TEntity>().Remove(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>>? filter)
        {
            return await _dbContext.Set<TEntity>().SingleOrDefaultAsync(filter) ?? 
                throw new NotFoundException("Entity not found.");
        }

        public async Task<TEntity> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(Id) ??
                throw new NotFoundException("Entity not found.");
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}