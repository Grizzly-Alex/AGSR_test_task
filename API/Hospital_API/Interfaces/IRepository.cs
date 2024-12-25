using System.Linq.Expressions;

namespace Hospital_API.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        Task<IList<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken token = default);
        Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        Task<TResult?> Get<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken token = default);

        Task<IEnumerable<TResult>> GetAll<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken token = default);

        Task<TEntity?> Create(TEntity entity, CancellationToken token = default);
        Task<IEnumerable<TEntity>?> Create(IEnumerable<TEntity> entities, CancellationToken token = default);
        Task<bool> Update(TEntity entity, CancellationToken token = default);
        Task<bool> Delete(TEntity entity, CancellationToken token = default);
    }
}
