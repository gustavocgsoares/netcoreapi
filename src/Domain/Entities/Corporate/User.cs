using System;
using System.Threading.Tasks;
using Template.Application.Interfaces.Base;
using Template.CrossCutting.Exceptions.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.CrossCutting.Resources.Validations;
using Template.Domain.Entities.Base;
using Template.Domain.Enums.Corporate;

namespace Template.Domain.Entities.Corporate
{
    public class User : Entity<User, Guid>
    {
        #region Constructors | Destructors
        public User()
        {
        }
        #endregion

        #region Properties
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Password { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual DateTime BirthDate { get; set; }

        public virtual string ProfileImage { get; set; }

        public virtual short AccessAttempts { get; set; }

        public virtual DateTime LastAccessDate { get; set; }

        public virtual DateTime LastAcceptanceTermsDate { get; set; }

        public virtual bool? Blocked { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<User, Guid> repository)
        {
            var user = await repository.GetFirstAsync(e => e.Email == Email);
            user.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task UpdateAsync(IRepository<User, Guid> repository)
        {
            var user = await repository.GetAsync(Id);
            user.IsNull().Throw<DataNotFoundException>(Id);

            if (Email.HasValue() && !user.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var userSameEmail = await repository.GetFirstAsync(e => e.Email == Email);
                userSameEmail.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

                user.Email = Email;
            }

            if (FirstName.HasValue())
            {
                user.FirstName = FirstName;
            }

            if (LastName.HasValue())
            {
                user.LastName = LastName;
            }

            if (Password.HasValue())
            {
                user.Password = Password;
            }

            user.Active = Active;
            ModifiedDate = DateTime.UtcNow;

            await repository.SaveAsync(user);
        }

        public override async Task DeleteAsync(IRepository<User, Guid> repository)
        {
            var user = await repository.GetAsync(Id);

            if (user.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
