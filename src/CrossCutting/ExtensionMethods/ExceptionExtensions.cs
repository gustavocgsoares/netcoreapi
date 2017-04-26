using System;

namespace Template.CrossCutting.ExtensionMethods
{
    public static class ExceptionExtensions
    {
        #region Public methods
        public static void Throw<TException>(this object obj, params object[] parameters)
            where TException : Exception
        {
            throw (Exception)Activator.CreateInstance(typeof(TException), parameters);
        }

        public static void Throw<TException>(this bool condition, params object[] parameters)
            where TException : Exception
        {
            if (condition)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), parameters);
            }
        }
        #endregion
    }
}
