using System;
using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class UnpredictableException : BaseException
    {
        #region Constructors | Destructors
        public UnpredictableException(Exception exception)
            : base(exception, Messages.UnpredictableException)
        {
            Code = "99999";
            Exception = exception;
        }
        #endregion

        #region Properties
        public Exception Exception { get; private set; }
        #endregion
    }
}
