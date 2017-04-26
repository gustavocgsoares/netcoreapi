using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;

namespace Template.Domain.Entities.Base
{
    public abstract class Entity<TEntity, TId> : BaseEntity
        where TEntity : Entity<TEntity, TId>
    {
        #region Fields | Members
        private int? requestedHashCode;
        #endregion

        #region Constructors | Destructors
        public Entity()
        {
            ////TODO: Metadata
            ////Metadata = new Dictionary<string, object>();
            Advices = new List<string>();
        }
        #endregion

        #region Properties
        public virtual TId Id { get; set; }

        public virtual DateTime AddedDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual DateTime? DeletedDate { get; set; }

        public virtual string IPAddress { get; set; }

        public virtual long Version { get; set; }

        public virtual bool Active { get; set; }

        ////public virtual IDictionary<string, object> Metadata { get; set; }

        [NotMapped]
        public List<string> Advices { get; set; }
        #endregion

        #region Public methods
        public void AddValidation(string message)
        {
            if (Advices == null)
            {
                Advices = new List<string>();
            }

            Advices.Add(message);
        }

        public abstract void ValidateProperties(Enums.Base.Action action);

        public virtual bool Validate(Enums.Base.Action action)
        {
            ValidateProperties(action);

            if (Advices == null)
            {
                Advices = new List<string>();
            }

            return Advices.Count.Equals(0);
        }

        public virtual Task AddAsync(IRepository<TEntity, TId> repository)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(IRepository<TEntity, TId> repository)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(IRepository<TEntity, TId> repository)
        {
            throw new NotImplementedException();
        }

        public virtual async Task SaveAsync(IRepository<TEntity, TId> repository)
        {
            if (IsTransient())
            {
                await AddAsync(repository);
            }
            else
            {
                await UpdateAsync(repository);
            }
        }

        public virtual bool IsTransient()
        {
            return EqualityComparer<TId>.Default.Equals(Id, default(TId));
        }

        public virtual T Clone<T>()
        {
            return (T)Clone();
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        #region Object overrides
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TEntity, TId>))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = (Entity<TEntity, TId>)obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }

            return EqualityComparer<TId>.Default.Equals(item.Id, Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                {
                    requestedHashCode = Id.GetHashCode() ^ 31;
                }

                return requestedHashCode.Value;
            }

            return base.GetHashCode();
        }
        #endregion
    }
}