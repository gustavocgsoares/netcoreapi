namespace Template.CrossCutting.ExtensionMethods
{
    public static class ClassExtensions
    {
        #region Public methods
        public static bool IsNull<TClass>(this TClass obj)
            where TClass : class
        {
            return obj == null;
        }

        public static bool IsNotNull<TClass>(this TClass obj)
            where TClass : class
        {
            return obj != null;
        }

        public static bool IsDifferent(this object source, object obj)
        {
            return (source ?? string.Empty).ToString() != (obj ?? string.Empty).ToString();
        }
        #endregion
    }
}
