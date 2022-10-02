using Autonoma.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Autonoma.Testing.IntegrationTests
{
    public class MainAPITest
    {
        [Fact]
        public void Test()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });

            var client = application.CreateClient();
            //...
        }
    }
}