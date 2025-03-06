using System.Linq.Expressions;

namespace StealAllTheCatsAssignment.Application.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);

        void Remove(TEntity entity);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<bool> RemoveById(int id);

        Task<TEntity?> Get(int id);

        Task<IEnumerable<TEntity>> GetAll();
    }
}
