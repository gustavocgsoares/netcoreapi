using System;
using Template.Application.Contexts.Base;
using Template.Application.Interfaces.Corporate;
using Template.Domain.Entities.Corporate;

namespace Template.Application.Contexts.Corporate
{
    public class UserApp
        : BaseCrudApp<User, Guid>, IUserApp
    {
        #region Fields | Members
        private readonly IUserRepository userRepository;
        #endregion

        #region Constructors | Destructors
        public UserApp(IUserRepository userRepository)
            : base(userRepository)
        {
            this.userRepository = userRepository;
        }
        #endregion
    }
}
