using System.Collections;
using System.Collections.Generic;

namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Base list model.
    /// </summary>
    /// <typeparam name="TListModel">Type of base list.</typeparam>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public abstract class BaseListModel<TListModel, TModel> : List<TModel>
        where TListModel : ICollection, new()
    {
        #region Static methods

        /// <summary>
        /// Get instance of list model.
        /// </summary>
        /// <returns>Instance of list model.</returns>
        public static TListModel Instance()
        {
            return new TListModel();
        }
        #endregion
    }
}