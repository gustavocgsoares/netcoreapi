using System;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Corporate;

namespace Template.Application.Interfaces.Corporate
{
    public interface IUserApp : IBaseCrudApp<User, Guid>
    {
    }
}
