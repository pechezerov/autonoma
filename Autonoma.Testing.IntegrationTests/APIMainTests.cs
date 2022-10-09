using Autonoma.API;
using Autonoma.API.Main.Client;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Main.Contracts.DataPoint;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace Autonoma.Testing.IntegrationTests
{
    public class APIMainTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly APIMainClient _httpClient;

        private const string baseUrl = "http://localhost:5000";

        public APIMainTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = new APIMainClient(baseUrl, webAppFactory.CreateDefaultClient());
        }

        [Fact]
        public async Task AdapterConfigurationList_CrudLifeCycle()
        {
            // Read
            var getListRequestObject = new AdapterConfigurationListQuery { };
            var getListResult = await _httpClient.AdaptersConfiguration_AdapterConfigurationAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);

            // Create
            var createAdapterRequestObject = new AdapterConfigurationItem
            {
                Address = "Test",
                Name = "Test",
                IpAddress = "",
                DataPoints = new List<DataPointConfigurationItem>
                {
                    new DataPointConfigurationItem { Name = "Test1", Unit = "test", Mapping = "test" },
                    new DataPointConfigurationItem { Name = "Test2", Unit = "test", Mapping = "test" }
                }
            };
            await _httpClient.AdaptersConfiguration_CreateAdapterConfigurationAsync(createAdapterRequestObject);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterConfigurationAsync(getListRequestObject);
            Assert.True(getListResult.Count == 1);
            Assert.True(getListResult.Data.Count() == 1);
            Assert.True(getListResult.Data.First().Name == "Test");
            var createdId = getListResult.Data.First().Id;

            // Update
            var updateAdapterRequestObject = new AdapterConfigurationItem
            {
                Id = createdId,
                Address = "UpdatedTest",
                Name = "UpdatedTest",
                IpAddress = "",
                DataPoints = new List<DataPointConfigurationItem>
                {
                    new DataPointConfigurationItem { Name = "UpdatedTest1", Unit = "updatedtest", Mapping = "updatedtest" },
                    new DataPointConfigurationItem { Name = "UpdatedTest3", Unit = "newtest", Mapping = "newtest" }
                }
            };
            await _httpClient.AdaptersConfiguration_UpdateAdapterConfigurationAsync(updateAdapterRequestObject);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterConfigurationAsync(getListRequestObject);
            Assert.True(getListResult.Count == 1);
            Assert.True(getListResult.Data.Count() == 1);
            Assert.True(getListResult.Data.First().Name == "UpdatedTest");

            // Delete
            await _httpClient.AdaptersConfiguration_DeleteAdapterConfigurationAsync(createdId);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterConfigurationAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);
        }
    }
}