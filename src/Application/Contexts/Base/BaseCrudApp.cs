using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.Domain.Entities.Base;
using Template.Domain.Enums.Base;

namespace Template.Application.Contexts.Base
{
    public abstract class BaseCrudApp<TEntity, TId> : BaseApp, IBaseCrudApp<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
    {
        #region Fields | Members
        private readonly IRepository<TEntity, TId> repository;
        #endregion

        #region Constructors | Destructors
        protected BaseCrudApp(IRepository<TEntity, TId> repository)
        {
            this.repository = repository;
        }

        ~BaseCrudApp()
        {
            Dispose(false);
        }
        #endregion

        #region IBaseCrudApp members
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            bool invalid = false;

            entities.ToList().ForEach(e =>
                {
                    if (!e.Validate(Action.Delete))
                    {
                        invalid = true;
                        return;
                    }
                });

            if (invalid)
            {
                return;
            }

            await repository.DeleteAsync(entities);
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                await entity.DeleteAsync(repository);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<PagedList<TEntity>> GetAllAsync(int index, int quantity, string ordering = null, bool ascending = true)
        {
            return await repository.GetAllAsync(index, quantity, ordering, ascending);
        }

        public async Task<TEntity> GetAsync(string id)
        {
            var decryptedId = id.To<TId>();
            return await repository.GetAsync(decryptedId);
        }

        public async Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities)
        {
            bool invalid = false;

            entities.ToList().ForEach(e =>
            {
                if ((e.IsTransient() && !e.Validate(Action.Add))
                || (!e.IsTransient() && !e.Validate(Action.Update)))
                {
                    invalid = true;
                    return;
                }
            });

            if (invalid)
            {
                return null;
            }

            return await repository.SaveAsync(entities);
        }

        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            await entity.SaveAsync(repository);
            return entity;
        }
        #endregion
    }
}
