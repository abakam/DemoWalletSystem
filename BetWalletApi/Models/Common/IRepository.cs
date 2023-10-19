using System.Linq.Expressions;

namespace BetWalletApi.Models.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        void Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync<TId>(TId id) where TId : notnull;
        Task<ICollection<TEntity>> ListAsync();
        Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate); 
    }
}
