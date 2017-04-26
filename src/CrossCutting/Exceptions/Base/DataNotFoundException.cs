using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class DataNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public DataNotFoundException(string data)
            : base(Messages.KeyNotFoundException, data)
        {
        }
        #endregion
    }
}
