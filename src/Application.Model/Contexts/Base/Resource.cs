using System.Collections.Generic;
using Newtonsoft.Json;

namespace Template.Application.Model.Contexts.Base
{
    /// <summary>
    /// Resource to do HATEOAS restful pattern.
    /// </summary>
    public abstract class Resource
    {
        /// <summary>
        /// Links to related resource.
        /// </summary>
        [JsonProperty(Order = -2)]
        public List<Link> Links { get; set; }
    }
}
