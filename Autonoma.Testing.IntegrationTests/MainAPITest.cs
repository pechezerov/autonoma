using Autofac.Extensions.DependencyInjection;
using Autonoma.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Autonoma.Testing.IntegrationTests
{
    public class MainAPITest
    {
        [Fact]
        public async Task Test()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                });

            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}