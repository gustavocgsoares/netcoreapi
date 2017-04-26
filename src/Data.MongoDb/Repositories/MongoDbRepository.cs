using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Template.Application.Interfaces.Base;
using Template.CrossCutting.Exceptions.Base;
using Template.Data.MongoDb.Helpers;
using Template.Domain.Entities.Base;

namespace Template.Data.MongoDb.Repositories.Base
{
    public abstract class MongoDbRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
    {
        #region Fields | Members
        private static IMongoClient client;

        private static IMongoDatabase database;

        private IMongoCollection<TEntity> collection;

        private bool disposed;
        #endregion

        #region Constructors | Destructors
        static MongoDbRepository()
        {
            ClassMapHelper.RegisterConventionPacks();
            ClassMapHelper.SetupClassMap<TEntity, TId>();
        }

        public MongoDbRepository(IOptions<CrossCutting.Configurations.Data> data, string collectionName)
        {
            var connectionString = data.Value.MongoDb.ConnectionString;
            var mongoUrl = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            client = new MongoClient(settings);
            database = client.GetDatabase(data.Value.MongoDb.Database);
            collection = database.GetCollection<TEntity>(collectionName);
        }

        ~MongoDbRepository()
        {
            Dispose(false);
        }
        #endregion

        #region IRepository members
        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await collection
                .Find(e => e.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(string ordering = null, bool ascending = true)
        {
            ////TODO: ordering field

            var query = collection.Find(e => true);

            return ascending
                 ? await query.SortBy(e => e.Id).ToListAsync()
                 : await query.SortByDescending(e => e.Id).ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int index, int limit, string ordering = null, bool ascending = true)
        {
            ////TODO: ordering field

            long totalItems = 0;

            var query = collection.Find(e => true);

            query = ascending
                  ? query.SortBy(e => e.Id)
                  : query.SortByDescending(e => e.Id);

            totalItems = query.Count();

            var result = await query
                .Skip(index)
                .Limit(limit)
                .ToListAsync();

            return new PagedList<TEntity>
            {
                Offset = index,
                Limit = limit,
                Items = query.ToList(),
                Total = totalItems
            };
        }

        public virtual async Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities)
        {
            await entities.AsQueryable().ForEachAsync(async e => await SaveAsync(e));
            return entities;
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (entity.IsTransient())
            {
                return await CreateAsync(entity);
            }
            else
            {
                return await UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            await entities.AsQueryable().ForEachAsync(async e => await DeleteAsync(e));
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await collection.FindOneAndDeleteAsync(e => e.Id.Equals(entity.Id));
        }
        #endregion

        #region Disposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
            }

            disposed = true;
        }
        #endregion

        #region Private methods
        private async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await collection.InsertOneAsync(entity, null);
                return await GetAsync(entity.Id);
            }
            catch (MongoWriteException)
            {
                throw new BusinessConflictException("Insert failed because the entity already exists!");
            }
        }

        private async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var previuosVersion = entity.Version;
            entity.Version++;

            ReplaceOneResult result;

            //// -> Find entity with same Id
            var idFilter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);

            //// -> Consistency enforcement
            if (!IgnoreVersion())
            {
                var versionLowerThan = Builders<TEntity>.Filter.Lt(e => e.Version, entity.Version);

                //// -> Consistency enforcement: Where current._id = entity.Id AND entity.Version > current.Version
                result = await collection.ReplaceOneAsync(
                    Builders<TEntity>.Filter.And(idFilter, versionLowerThan),
                    entity,
                    null);

                if (result != null && ((result.IsAcknowledged && result.MatchedCount == 0) || (result.IsModifiedCountAvailable && !(result.ModifiedCount > 0))))
                {
                    throw new BusinessConflictException("Update failed because entity versions conflict!");
                }
            }
            else
            {
                result = await collection.ReplaceOneAsync(idFilter, entity, null);

                if (result != null && ((result.IsAcknowledged && result.MatchedCount == 0) || (result.IsModifiedCountAvailable && !(result.ModifiedCount > 0))))
                {
                    throw new DataNotFoundException(entity.Id.ToString());
                }
            }

            return await GetAsync(entity.Id);
        }

        private bool IgnoreVersion()
        {
            return false;
        }
        #endregion
    }
}