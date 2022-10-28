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
            var getListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);

            // Create
            var createAdapterRequestObject = new AdapterConfigurationItem
            {
                Address = "Test",
                Name = "Test",
                IpAddress = ""
            };
            await _httpClient.AdaptersConfiguration_CreateAdapterAsync(createAdapterRequestObject);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getListRequestObject);
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
                IpAddress = ""
            };
            await _httpClient.AdaptersConfiguration_UpdateAdapterConfigurationAsync(updateAdapterRequestObject);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 1);
            Assert.True(getListResult.Data.Count() == 1);
            Assert.True(getListResult.Data.First().Name == "UpdatedTest");

            // Delete
            await _httpClient.AdaptersConfiguration_DeleteAdapterAsync(createdId);
            getListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);
        }

        [Fact]
        public async Task DataPointConfigurationList_CrudLifeCycle()
        {
            // Read
            var getListRequestObject = new DataPointConfigurationListQuery { };
            var getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);

            // Create
            var createDataPointRequestObject = new DataPointConfigurationItem
            {
                Name = "Test",
                Mapping = "Test",
            };
            await _httpClient.DataPointsConfiguration_CreateDataPointAsync(createDataPointRequestObject);
            getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 1);
            Assert.True(getListResult.Data.Count() == 1);
            Assert.True(getListResult.Data.First().Name == "Test");
            var createdId = getListResult.Data.First().Id;

            // Update
            var updateDataPointRequestObject = new DataPointConfigurationItem
            {
                Id = createdId,
                Name = "UpdatedTest",
                Mapping = "UpdatedTest",
            };
            await _httpClient.DataPointsConfiguration_UpdateDataPointAsync(updateDataPointRequestObject);
            getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 1);
            Assert.True(getListResult.Data.Count() == 1);
            Assert.True(getListResult.Data.First().Name == "UpdatedTest");

            // Delete
            await _httpClient.DataPointsConfiguration_DeleteDataPointAsync(createdId);
            getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);
        }
    }
}