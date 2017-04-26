using System.Collections.Generic;
using Template.Application.Model.Contexts.Base;

namespace Template.Application.Model.Contexts.V1.Security
{
    /// <summary>
    /// Define a login data.
    /// </summary>
    public class LoginModel : BaseModel<LoginModel>
    {
        #region Properties
        #endregion

        #region Converters

        /// <summary>
        /// Convert to <see cref="LoginModel"/>.
        /// </summary>
        /// <param name="links">See <see cref="List{T}"/> of <see cref="Link"/>.</param>
        /// <returns>See <see cref="LoginModel"/>.</returns>
        public static LoginModel ToModel(List<Link> links = null)
        {
            var model = Instance();

            model.Links = links;

            return model;
        }
        #endregion
    }
}
