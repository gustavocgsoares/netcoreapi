using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class CacheValueNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public CacheValueNotFoundException(string value)
            : base(Messages.CacheValueNotFoundException, value)
        {
        }
        #endregion
    }
}
