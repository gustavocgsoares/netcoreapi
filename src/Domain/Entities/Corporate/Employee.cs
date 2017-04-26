using System;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Base;

namespace Template.Domain.Entities.Corporate
{
    public class Employee : Entity<Employee, Guid>
    {
        #region Constructors | Destructors
        public Employee()
        {
        }
        #endregion

        #region Properties
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string IdentityDocument { get; set; }

        public virtual string SocialSecurity { get; set; }

        public virtual Guid UpdatingEmployeeId { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Employee, Guid> repository)
        {
            await repository.SaveAsync(this);
        }
        #endregion
    }
}
