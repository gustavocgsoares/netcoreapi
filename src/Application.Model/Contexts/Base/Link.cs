using Newtonsoft.Json;
using Template.Application.Model.Enums.Base;

namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Link to related resource.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Resource href.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Relation name to resource id.
        /// </summary>
        [JsonProperty(PropertyName = "rel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Relation { get; set; }

        /// <summary>
        /// Http method.
        /// </summary>
        public Method Method { get; set; }
    }
}
