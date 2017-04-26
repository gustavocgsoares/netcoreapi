using System;
using Template.Application.Interfaces.Base;
using Template.Domain.Entities.Support;

namespace Template.Application.Interfaces.Support
{
    public interface ISuggestionApp : IBaseCrudApp<Suggestion, Guid>
    {
    }
}
