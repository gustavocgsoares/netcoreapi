using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Template.Services.Web.Api.Tests.Base
{
    public class ClassFixture : IDisposable
    {
        #region Constructors | Destructors
        public ClassFixture()
        {
            var path = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            var builder = new WebHostBuilder()
                .UseContentRoot(path)
                .UseEnvironment("testing")
                .UseStartup<Startup>();

            TestServer = new TestServer(builder);
            Client = TestServer.CreateClient();
        }
        #endregion

        #region Properties
        public TestServer TestServer { get; }

        public HttpClient Client { get; }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
