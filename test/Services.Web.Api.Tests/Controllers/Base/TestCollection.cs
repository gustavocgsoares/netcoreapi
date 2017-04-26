using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.Base
{
    [CollectionDefinition("TestContext")]
    public class TestCollection : ICollectionFixture<CollectionFixture>
    {
        public TestCollection()
        {
        }
    }
}
