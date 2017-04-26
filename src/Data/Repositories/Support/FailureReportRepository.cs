using Template.Application.Interfaces.Support;
using Template.Data.SqlServer.Helpers;
using Template.Data.SqlServer.Repositories;
using Template.Domain.Entities.Support;

namespace Template.Data.Repositories.Support
{
    public class FailureReportRepository
        : SqlServerRepository<FailureReport, string>, IFailureReportRepository
    {
        #region Constructors | Destructors
        public FailureReportRepository(IQueryableUnitOfWork unitOfWork)
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
