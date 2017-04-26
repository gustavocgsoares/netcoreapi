using System;
using Template.Application.Interfaces.Corporate;
using Template.Data.SqlServer.Helpers;
using Template.Data.SqlServer.Repositories;
using Template.Domain.Entities.Corporate;

namespace Template.Data.Repositories.Corporate
{
    public class EmployeeRepository
        : SqlServerRepository<Employee, Guid>, IEmployeeRepository
    {
        #region Constructors | Destructors
        public EmployeeRepository(IQueryableUnitOfWork unitOfWork)
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
