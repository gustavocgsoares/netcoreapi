using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Template.Services.Web.Api.Tests.Base
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            // I'm only accepting JSON from the server, and I don't want to add a dependency on
            // System.Runtime.Serialization.Xml which is required when using the default formatters
            return await content.ReadAsAsync<T>(GetJsonFormatters());
        }

        private static IEnumerable<MediaTypeFormatter> GetJsonFormatters()
        {
            yield return new JsonMediaTypeFormatter();
        }
    }
}
