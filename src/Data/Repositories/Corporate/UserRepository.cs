using System;
using Microsoft.Extensions.Options;
using Template.Application.Interfaces.Corporate;
using Template.Data.MongoDb.Repositories.Base;
using Template.Data.SqlServer.Helpers;
using Template.Domain.Entities.Corporate;

namespace Template.Data.Repositories.Corporate
{
    public class UserRepository
        : MongoDbRepository<User, Guid>, IUserRepository
    {
        #region Constructors | Destructors
        public UserRepository(IQueryableUnitOfWork unitOfWork, IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "users")
        {
        }
        #endregion
    }
}
