using System;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Base;
using Template.Domain.ValueObjects;

namespace Template.Domain.Entities.Support
{
    public class FailureReport : Entity<FailureReport, string>
    {
        #region Constructors | Destructors
        public FailureReport()
        {
        }
        #endregion

        #region Properties
        public virtual string Title { get; set; }

        public virtual string Message { get; set; }

        public virtual PlatformVo Platform { get; set; }

        public virtual string UserId { get; set; }

        public object Report { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<FailureReport, string> repository)
        {
            await repository.SaveAsync(this);
        }
        #endregion
    }
}
