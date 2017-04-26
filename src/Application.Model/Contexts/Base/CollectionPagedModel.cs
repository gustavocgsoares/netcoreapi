namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Base collection model paged.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public abstract class CollectionPagedModel<TModel> : CollectionModel<TModel>
    {
        /// <summary>
        /// Total items into database.
        /// </summary>
        public virtual long TotalCount { get; set; }

        /// <summary>
        /// Total pages according offset and limit sent.
        /// </summary>
        public virtual int TotalPages { get; set; }
    }
}
