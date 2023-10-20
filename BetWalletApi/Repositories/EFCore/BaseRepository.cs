using BetWalletApi.Models.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BetWalletApi.Repositories.EFCore
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BetWalletDbContext _dbContext;

        public BaseRepository(BetWalletDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }   

        public virtual TEntity Add(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Add(entity).Entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> GetByIdAsync<TId>(TId id) where TId : notnull
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<ICollection<TEntity>> ListAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);

            return await _dbContext.SaveChangesAsync();
        }
    }
}
