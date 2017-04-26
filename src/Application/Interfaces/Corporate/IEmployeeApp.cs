using System;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Corporate;

namespace Template.Application.Interfaces.Corporate
{
    public interface IEmployeeApp : IBaseCrudApp<Employee, Guid>
    {
    }
}
