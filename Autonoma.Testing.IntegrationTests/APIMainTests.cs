using Autonoma.API;
using Autonoma.API.Main.Client;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Main.Contracts.DataPoint;
using Microsoft.AspNetCore.Mvc.Testing;

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
        public async Task AdapterConfiguration_CrudLifeCycle()
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
        public async Task AdapterConfiguration_ReadList()
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
            for (int i = 0; i < 10; i++)
                await _httpClient.AdaptersConfiguration_CreateAdapterAsync(createAdapterRequestObject);

            // Read all by page
            var getListResult1 = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { PageSize = 4, PageIndex = 1 });
            Assert.True(getListResult1.Count == 10);
            Assert.True(getListResult1.Data.Count() == 4);

            var getListResult2 = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { PageSize = 4, PageIndex = 2 });
            Assert.True(getListResult2.Count == 10);
            Assert.True(getListResult2.Data.Count() == 4);

            var getListResult3 = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { PageSize = 4, PageIndex = 3 });
            Assert.True(getListResult3.Count == 10);
            Assert.True(getListResult3.Data.Count() == 2);
            
            // Read all
            var getListResultFull = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { PageSize = 20 });
            Assert.True(getListResultFull.Count == 10);
            Assert.True(getListResultFull.Data.Count() == 10);

            // Read by ids
            var getListResultByIds = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { Ids = new[] { 4, 5, 6 } });
            Assert.True(getListResultByIds.Count == 3);
            Assert.True(getListResultByIds.Data.Count() == 3);
            Assert.True(getListResultByIds.Data.Select(a => a.Id)
                .OrderBy(id => id)
                .ToArray().SequenceEqual(new[] { 4, 5, 6 }));

            // Read by ids by page
            var getListResultPageByIds1 = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { Ids = new[] { 4, 5, 6 }, PageSize = 2 });
            Assert.True(getListResultPageByIds1.Count == 3);
            Assert.True(getListResultPageByIds1.Data.Count() == 2);
            var getListResultPageByIds2 = await _httpClient.AdaptersConfiguration_AdapterListAsync(new AdapterConfigurationListQuery { Ids = new[] { 4, 5, 6 }, PageSize = 2, PageIndex = 2 });
            Assert.True(getListResultPageByIds2.Count == 3);
            Assert.True(getListResultPageByIds2.Data.Count() == 1);

            await _httpClient.Administration_ResetSystemAsync();
        }

        [Fact]
        public async Task DataPointConfiguration_CrudLifeCycle()
        {
            // Create adapter before work with datapoints
            var createAdapterRequestObject = new AdapterConfigurationItem
            {
                Address = "Test",
                Name = "Test",
                IpAddress = ""
            };
            await _httpClient.AdaptersConfiguration_CreateAdapterAsync(createAdapterRequestObject);
            var getAdapterListRequestObject = new AdapterConfigurationListQuery { };
            var getAdapterListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getAdapterListRequestObject);
            var adapterId = getAdapterListResult.Data.First().Id;

            // Read
            var getListRequestObject = new DataPointConfigurationListQuery { };
            var getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);

            // Create
            var createDataPointRequestObject = new DataPointConfigurationItem
            {
                AdapterId = adapterId,
                Name = "Test",
                Mapping = "Test",
                Unit = "Test"
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
                AdapterId = adapterId,
                Id = createdId,
                Name = "UpdatedTest",
                Mapping = "UpdatedTest",
                Unit = "UpdatedTest"
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

        [Fact]
        public async Task DataPointConfiguration_ReadList()
        {
            // Create adapter before work with datapoints
            var createAdapterRequestObject = new AdapterConfigurationItem
            {
                Address = "Test",
                Name = "Test",
                IpAddress = ""
            };
            await _httpClient.AdaptersConfiguration_CreateAdapterAsync(createAdapterRequestObject);
            var getAdapterListRequestObject = new AdapterConfigurationListQuery { };
            var getAdapterListResult = await _httpClient.AdaptersConfiguration_AdapterListAsync(getAdapterListRequestObject);
            var adapterId = getAdapterListResult.Data.First().Id;

            // Read
            var getListRequestObject = new DataPointConfigurationListQuery { };
            var getListResult = await _httpClient.DataPointsConfiguration_DataPointListAsync(getListRequestObject);
            Assert.True(getListResult.Count == 0);
            Assert.True(getListResult.Data.Count() == 0);

            // Create
            var createDataPointRequestObject = new DataPointConfigurationItem
            {
                AdapterId = adapterId,
                Name = "Test",
                Mapping = "Test",
                Unit = "Test"
            };
            for (int i = 0; i < 10; i++)
                await _httpClient.DataPointsConfiguration_CreateDataPointAsync(createDataPointRequestObject);

            // Read all by page
            var getListResult1 = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { PageSize = 4, PageIndex = 1 });
            Assert.True(getListResult1.Count == 10);
            Assert.True(getListResult1.Data.Count() == 4);

            var getListResult2 = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { PageSize = 4, PageIndex = 2 });
            Assert.True(getListResult2.Count == 10);
            Assert.True(getListResult2.Data.Count() == 4);

            var getListResult3 = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { PageSize = 4, PageIndex = 3 });
            Assert.True(getListResult3.Count == 10);
            Assert.True(getListResult3.Data.Count() == 2);

            // Read all
            var getListResultFull = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { PageSize = 20 });
            Assert.True(getListResultFull.Count == 10);
            Assert.True(getListResultFull.Data.Count() == 10);

            // Read by ids
            var getListResultByIds = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { Ids = new[] { 4, 5, 6 } });
            Assert.True(getListResultByIds.Count == 3);
            Assert.True(getListResultByIds.Data.Count() == 3);
            Assert.True(getListResultByIds.Data.Select(a => a.Id)
                .OrderBy(id => id)
                .ToArray().SequenceEqual(new[] { 4, 5, 6 }));

            // Read by ids by page
            var getListResultPageByIds1 = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { Ids = new[] { 4, 5, 6 }, PageSize = 2 });
            Assert.True(getListResultPageByIds1.Count == 3);
            Assert.True(getListResultPageByIds1.Data.Count() == 2);
            var getListResultPageByIds2 = await _httpClient.DataPointsConfiguration_DataPointListAsync(new DataPointConfigurationListQuery { Ids = new[] { 4, 5, 6 }, PageSize = 2, PageIndex = 2 });
            Assert.True(getListResultPageByIds2.Count == 3);
            Assert.True(getListResultPageByIds2.Data.Count() == 1);

            await _httpClient.Administration_ResetSystemAsync();
        }
    }
}