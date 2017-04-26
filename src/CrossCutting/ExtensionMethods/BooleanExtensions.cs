namespace Template.CrossCutting.ExtensionMethods
{
    public static class BooleanExtensions
    {
        #region Public methods
        public static bool Not(this bool flag)
        {
            return !flag;
        }
        #endregion
    }
}
