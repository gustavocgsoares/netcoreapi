using System;
using Template.Application.Interfaces.Support;
using Template.Data.SqlServer.Helpers;
using Template.Data.SqlServer.Repositories;
using Template.Domain.Entities.Support;

namespace Template.Data.Repositories.Support
{
    public class SuggestionRepository
        : SqlServerRepository<Suggestion, Guid>, ISuggestionRepository
    {
        #region Constructors | Destructors
        public SuggestionRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #endregion

        #region SqlServerRepository overrides
        public override void DisposeItems()
        {
        }
        #endregion
    }
}
