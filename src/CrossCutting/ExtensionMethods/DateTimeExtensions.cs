using System;

namespace Template.CrossCutting.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        #region Public methods
        public static DateTime Custom(this DateTime dateTime)
        {
            return DateTime.Now;
        }

        public static DateTime SqlMinValue(this DateTime dateTime)
        {
            return new DateTime(1753, 1, 1);
        }
        #endregion
    }
}
