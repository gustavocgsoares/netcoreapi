using Template.Application.Contexts.Base;
using Template.Application.Interfaces.Support;
using Template.Domain.Entities.Support;

namespace Template.Application.Contexts.Support
{
    public class FailureReportApp
        : BaseCrudApp<FailureReport, string>, IFailureReportApp
    {
        #region Fields | Members
        private readonly IFailureReportRepository failureReportRepository;
        #endregion

        #region Constructors | Destructors
        public FailureReportApp(IFailureReportRepository failureReportRepository)
            : base(failureReportRepository)
        {
            this.failureReportRepository = failureReportRepository;
        }
        #endregion
    }
}
