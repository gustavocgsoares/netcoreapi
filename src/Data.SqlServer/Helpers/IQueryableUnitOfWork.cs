using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Application.Interfaces.Base;

namespace Template.Data.SqlServer.Helpers
{
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class;

        Task AttachAsync<TEntity>(TEntity item)
            where TEntity : class;

        void SetModified<TEntity>(TEntity item)
            where TEntity : class;

        Task ApplyCurrentValuesAsync<TEntity>(TEntity original, TEntity current)
            where TEntity : class;
    }
}