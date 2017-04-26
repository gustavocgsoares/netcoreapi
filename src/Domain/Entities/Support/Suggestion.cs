using System;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Base;

namespace Template.Domain.Entities.Support
{
    public class Suggestion : Entity<Suggestion, Guid>
    {
        #region Constructors | Destructors
        public Suggestion()
        {
        }
        #endregion

        #region Properties
        public virtual string User { get; set; }

        public virtual string Title { get; set; }

        public virtual string Message { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Suggestion, Guid> repository)
        {
            await repository.SaveAsync(this);
        }
        #endregion
    }
}
