using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Data.SqlServer.Models;

namespace Template.Data.SqlServer.Helpers
{
    public class UnitOfWork : TemplateDbContext, IQueryableUnitOfWork
    {
        #region Fields | Members
        ////private bool _disposed;
        #endregion

        #region Constructors | Destructors
        public UnitOfWork(DbContextOptions<TemplateDbContext> options, IOptions<CrossCutting.Configurations.Data> data)
            : base(options, data)
        {
        }

        ~UnitOfWork()
        {
            Dispose();
        }
        #endregion

        #region IQueryableUnitOfWork Members
        public async Task ApplyCurrentValuesAsync<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            ////If it is not attached, attach original and set current values.
            await Task.Factory.StartNew(() => Entry(original).CurrentValues.SetValues(current));
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            await SaveChangesAsync(ct);
        }

        public async Task CommitAndRefreshChangesAsync(CancellationToken ct)
        {
            bool saveFailed;

            do
            {
                try
                {
                    await SaveChangesAsync(ct);
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException)
                {
                    saveFailed = true; ////TODO: DbUpdateConcurrencyException
                    ////ex.Entries.ToList().ForEach(entry => entry.Context.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            }
            while (saveFailed);
        }

        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return Set<TEntity>();
        }

        public void RollbackChanges()
        {
            ////Set all entities in change tracker as 'unchanged state'.
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void SetConnection(string connectionString)
        {
            ////Unity doesn'allow several injected after the generation of the graph...
            if (!string.IsNullOrEmpty(connectionString))
            {
                ////Database.Connection.ConnectionString = connectionString;
                throw new NotImplementedException();
            }
        }

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            ////This operation also attach item in object state manager.
            Entry(item).State = EntityState.Modified;
        }

        public Task AttachAsync<TEntity>(TEntity item)
            where TEntity : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
