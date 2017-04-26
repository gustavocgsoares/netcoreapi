using System;
using Template.Application.Contexts.Base;
using Template.Application.Interfaces.Corporate;
using Template.Domain.Entities.Corporate;

namespace Template.Application.Contexts.Corporate
{
    public class EmployeeApp
        : BaseCrudApp<Employee, Guid>, IEmployeeApp
    {
        #region Fields | Members
        private readonly IEmployeeRepository employeeRepository;
        #endregion

        #region Constructors | Destructors
        public EmployeeApp(IEmployeeRepository employeeRepository)
            : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        #endregion
    }
}
