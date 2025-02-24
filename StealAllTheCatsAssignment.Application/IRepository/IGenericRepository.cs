using StealAllTheCatsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
