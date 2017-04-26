using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class CacheNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public CacheNotFoundException(string cacheName)
            : base(Messages.CacheNotFoundException, cacheName)
        {
        }
        #endregion
    }
}
