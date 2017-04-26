using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Template.CrossCutting.ExtensionMethods;
using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.Base
{
    public abstract class BaseConfiguration : IClassFixture<ClassFixture>
    {
        private readonly HttpClient client;

        public BaseConfiguration(ClassFixture fixture)
        {
            client = fixture.Client;
        }

        public async Task<HttpResponseMessage> RequestingApiAsync(HttpMethod method, string requestUri)
        {
            return await RequestingApiAsync<dynamic>(method, requestUri, null);
        }

        public async Task<HttpResponseMessage> RequestingApiAsync<TModel>(HttpMethod method, string requestUri, TModel body = null)
            where TModel : class
        {
            switch (method.Method)
            {
                case "DELETE":
                    return await client.DeleteAsync(requestUri);
                case "POST":
                    if (body.IsNotNull())
                    {
                        return await client.PostAsync(requestUri, body, new JsonMediaTypeFormatter());
                    }

                    return await client.PostAsync(requestUri, null);
                case "PUT":
                    if (body.IsNotNull())
                    {
                        return await client.PutAsync(requestUri, body, new JsonMediaTypeFormatter());
                    }

                    return await client.PutAsync(requestUri, null);
                case "GET":
                default:
                    return await client.GetAsync(requestUri);
            }
        }

        public async Task<TModel> WhenGetContentAsModelAsync<TModel>(HttpResponseMessage response)
        {
            return await response.Content.ReadAsJsonAsync<TModel>();
        }
    }
}
