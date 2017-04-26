using System;
using Template.Application.Model.Contexts.Base;
using Template.Application.Model.Enums.V1.Corporate;
using Template.CrossCutting.ExtensionMethods;
using Template.Domain.Entities.Corporate;

namespace Template.Application.Model.Contexts.V1.Corporate
{
    /// <summary>
    /// Define a user.
    /// </summary>
    public class UserModel : BaseModel<UserModel>
    {
        #region Properties

        /// <summary>
        /// User id.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// User gender.
        /// </summary>
        public virtual Gender? Gender { get; set; }

        /// <summary>
        /// Birth date.
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Profile image.
        /// </summary>
        public virtual string ProfileImage { get; set; }

        /// <summary>
        /// Define if user already exists.
        /// Used when the user logs in via social network (facebook, google) and at another time, log in via email.
        /// </summary>
        public virtual bool? AlreadyExists { get; set; }

        /// <summary>
        /// How the user logged in.
        /// </summary>
        public virtual UserLoggedWith LoggedWith { get; set; }

        /// <summary>
        /// Last date and time of access.
        /// </summary>
        public virtual DateTime? LastAccessDate { get; set; }

        /// <summary>
        /// Last date and time of acceptance terms.
        /// </summary>
        public virtual DateTime? LastAcceptanceTermsDate { get; set; }

        /// <summary>
        /// Define if user is blocked.
        /// </summary>
        public virtual bool? Blocked { get; set; }
        #endregion

        #region Converters

        /// <summary>
        /// Convert <see cref="User"/> to <see cref="UserModel"/>.
        /// </summary>
        /// <param name="entity">See <see cref="User"/>.</param>
        /// <returns>See <see cref="UserModel"/>.</returns>
        public static UserModel ToModel(User entity)
        {
            var model = Instance();

            model.Id = entity.Id != Guid.Empty ? entity.Id.ToString() : null;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Email = entity.Email;
            model.Gender = (Gender)entity.Gender;
            model.BirthDate = entity.BirthDate;
            model.ProfileImage = entity.ProfileImage;
            model.LastAcceptanceTermsDate = entity.LastAcceptanceTermsDate;
            model.Blocked = entity.Blocked;

            return model;
        }

        /// <summary>
        /// Convert <see cref="UserModel"/> to <see cref="User"/>.
        /// </summary>
        /// <returns>See <see cref="User"/>.</returns>
        public User ToDomain()
        {
            return ToDomain(new User());
        }

        /// <summary>
        /// Convert <see cref="UserModel"/> to <see cref="User"/>.
        /// </summary>
        /// <param name="entity">Item <see cref="User"/> to be completed.</param>
        /// <returns>See <see cref="User"/>.</returns>
        public User ToDomain(User entity)
        {
            entity = entity ?? new User();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : Guid.Empty;
            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.Email = Email;
            entity.Gender = (Domain.Enums.Corporate.Gender)Gender;
            entity.BirthDate = BirthDate.GetValueOrDefault();
            entity.ProfileImage = ProfileImage;
            entity.LastAcceptanceTermsDate = LastAcceptanceTermsDate.GetValueOrDefault();
            entity.Blocked = Blocked;

            return entity;
        }
        #endregion
    }
}
