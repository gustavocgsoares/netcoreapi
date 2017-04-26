using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Base;

namespace Template.Data.SqlServer.Models.Extensions
{
    public static class ContextExtensions
    {
        public static EntityState GetState<TEntity, TId>(this Entity<TEntity, TId> entity)
            where TEntity : Entity<TEntity, TId>
        {
            return entity.IsTransient() ? EntityState.Added : EntityState.Modified;
        }
    }
}