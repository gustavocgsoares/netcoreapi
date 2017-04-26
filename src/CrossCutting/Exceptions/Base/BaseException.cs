using System;

namespace Template.CrossCutting.Exceptions.Base
{
    public abstract class BaseException : Exception
    {
        #region Constructors | Destructors
        public BaseException(string message, params string[] args)
            : base(string.Format(message, args))
        {
            Code = GetType().Name;
        }

        public BaseException(Exception innerException, string message, params string[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = GetType().Name;
        }
        #endregion

        #region Properties
        public string Code { get; protected set; }
        #endregion
    }
}
