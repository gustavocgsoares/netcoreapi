using System;
using Template.Application.Contexts.Base;
using Template.Application.Interfaces.Support;
using Template.Domain.Entities.Support;

namespace Template.Application.Contexts.Support
{
    public class SuggestionApp
        : BaseCrudApp<Suggestion, Guid>, ISuggestionApp
    {
        #region Fields | Members
        private readonly ISuggestionRepository suggestionRepository;
        #endregion

        #region Constructors | Destructors
        public SuggestionApp(ISuggestionRepository suggestionRepository)
            : base(suggestionRepository)
        {
            this.suggestionRepository = suggestionRepository;
        }
        #endregion
    }
}
