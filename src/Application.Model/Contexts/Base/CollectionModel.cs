namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Base collection model.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public abstract class CollectionModel<TModel> : Resource
    {
        /// <summary>
        /// Model items.
        /// </summary>
        public TModel[] Items { get; set; }
    }
}
