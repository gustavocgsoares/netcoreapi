namespace Template.CrossCutting.ExtensionMethods
{
    public static class IntegerExtensions
    {
        #region Public methods
        public static bool GreaterThan(this int source, int value)
        {
            return source > value;
        }

        public static bool GreaterThanOrEqualTo(this int source, int value)
        {
            return source >= value;
        }

        public static bool LessThan(this int source, int value)
        {
            return source < value;
        }

        public static bool LessThanOrEqualTo(this int source, int value)
        {
            return source <= value;
        }
        #endregion
    }
}
