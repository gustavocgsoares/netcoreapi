using System.Collections.Generic;
using Newtonsoft.Json;

namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Bad request model.
    /// </summary>
    public class BadRequestModel
    {
        #region Properties

        /// <summary>
        /// Error code.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Error validations.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public virtual List<string> Validations { get; set; }
        #endregion
    }
}