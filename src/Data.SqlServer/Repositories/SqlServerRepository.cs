using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Application.Interfaces.Base;
using Template.Data.SqlServer.Helpers;
using Template.Domain.Entities.Base;

namespace Template.Data.SqlServer.Repositories
{
    public abstract class SqlServerRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
    {
        #region Fields | Members
        private readonly IQueryableUnitOfWork unitOfWork;

        private bool disposed;
        #endregion

        #region Constructors | Destructors
        public SqlServerRepository(IQueryableUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        ~SqlServerRepository()
        {
            Dispose(false);
        }
        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }
        #endregion

        #region IRepository members
        public virtual async Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities)
        {
            await entities.AsQueryable().ForEachAsync(async e => await SaveAsync(e));
            return entities;
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (entity.IsTransient())
            {
                await GetSet().AddAsync(entity);
            }
            else
            {
                unitOfWork.SetModified(entity);
            }

            return await GetAsync(entity.Id);
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            await entities.AsQueryable().ForEachAsync(async e => await DeleteAsync(e));
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            var dbSet = GetSet();
            await dbSet.FindAsync(entity.Id);
            dbSet.Remove(entity);
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await GetSet().FirstOrDefaultAsync(item => item.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetSet().FirstOrDefaultAsync(filter);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(string ordering = null, bool ascending = true)
        {
            var dbSet = GetSet();
            var query = dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                query = query.OrderBy(ordering, ascending);
            }

            return await dbSet.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int index, int limit, string ordering = null, bool ascending = true)
        {
            var totalItems = 0;

            var query = await GetAllAsync(ordering, ascending);
            totalItems = query.Count();

            query = query
                .Skip(index)
                .Take(limit);

            return new PagedList<TEntity>
            {
                Offset = index,
                Limit = limit,
                Items = query.ToList(),
                Total = totalItems
            };
        }
        #endregion

        #region Disposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void DisposeItems();

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (unitOfWork != null)
                {
                    unitOfWork.Dispose();
                }

                DisposeItems();

                disposed = true;
            }

            disposed = true;
        }
        #endregion

        #region Private methods
        private DbSet<TEntity> GetSet()
        {
            return unitOfWork.CreateSet<TEntity>();
        }
        #endregion
    }
}