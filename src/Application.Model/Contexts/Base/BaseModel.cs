namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Base model.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public abstract class BaseModel<TModel> : Resource
        where TModel : new()
    {
        #region Static methods

        /// <summary>
        /// Get instance of model.
        /// </summary>
        /// <returns>Instance of model.</returns>
        public static TModel Instance()
        {
            return new TModel();
        }
        #endregion
    }
}