using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class KeyNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public KeyNotFoundException(string key)
            : base(Messages.KeyNotFoundException, key)
        {
        }
        #endregion
    }
}
