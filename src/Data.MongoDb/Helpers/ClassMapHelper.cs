using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Template.Domain.Entities.Base;

namespace Template.Data.MongoDb.Helpers
{
    public static class ClassMapHelper
    {
        #region Fields | Members
        private static object lockObject = new object();
        #endregion

        #region Public methods
        public static void RegisterConventionPacks()
        {
            lock (lockObject)
            {
                var conventionPack = new ConventionPack();
                conventionPack.Add(new IgnoreIfNullConvention(true));
                ConventionRegistry.Register("ConventionPack", conventionPack, t => true);
            }
        }

        public static void SetupClassMap<TEntity, TId>()
            where TEntity : Entity<TEntity, TId>
        {
            lock (lockObject)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
                {
                    BsonClassMap.RegisterClassMap<TEntity>(
                        (classMap) =>
                        {
                            classMap.AutoMap();
                            classMap.SetIdMember(classMap.GetMemberMap(a => a.Id));
                            classMap.SetDiscriminator(typeof(TEntity).Name);
                            ////classMap.MapExtraElementsMember(p => classMap.GetMemberMap(a => a.Metadata));
                        });
                }
            }
        }
        #endregion
    }
}