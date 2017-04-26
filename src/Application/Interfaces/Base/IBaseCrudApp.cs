using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.Entities.Base;

namespace Template.Application.Interfaces.Base
{
    public interface IBaseCrudApp<TEntity, TId> : IDisposable
        where TEntity : Entity<TEntity, TId>
    {
        Task<TEntity> GetAsync(string id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<PagedList<TEntity>> GetAllAsync(int index, int quantity, string ordering = null, bool ascending = true);

        Task<TEntity> SaveAsync(TEntity entity);

        Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(string id);

        Task DeleteAsync(IEnumerable<TEntity> entities);
    }
}
