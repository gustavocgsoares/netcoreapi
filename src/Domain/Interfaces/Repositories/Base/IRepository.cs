using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Template.Domain.Entities.Base;

namespace Template.Application.Interfaces.Base
{
    public interface IRepository<TEntity, in TId> : IDisposable
        where TEntity : Entity<TEntity, TId>
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetAsync(TId id);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetAllAsync(string ordering = null, bool ascending = true);

        Task<PagedList<TEntity>> GetAllAsync(int index, int limit, string ordering = null, bool ascending = true);
    }
}