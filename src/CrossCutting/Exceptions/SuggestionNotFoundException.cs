using Template.CrossCutting.Exceptions.Base;

namespace Template.CrossCutting.Exceptions
{
    public class SuggestionNotFoundException : BusinessRuleException
    {
        #region Constructors | Destructors
        public SuggestionNotFoundException(string cacheName)
            : base("00002", Resources.Exceptions.Base.Messages.CacheNotFoundException, cacheName)
        {
        }
        #endregion
    }
}
