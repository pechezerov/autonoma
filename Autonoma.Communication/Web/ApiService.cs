using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication.Web
{
    public abstract class ApiService
    {
        private readonly ILogger _logger;

        private readonly string? _apiEndpointUrl;

        private readonly string? _authEndpointUrl;

        private readonly string? _authClientId;

        private readonly string? _authSecret;

        private readonly string? _scopes;

        private readonly HttpClient _httpClient;

        private string? _accessToken;

        private string? _refreshToken;

        private DateTimeOffset _accessTokenLifetime;

        private readonly SemaphoreSlim _authorizeSlim;

        public ApiService(ILogger<ApiService> logger, ApiServiceSettings defaultSettings, ApiServiceSettings settings, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _apiEndpointUrl = settings.ApiEndpointUrl ?? defaultSettings.ApiEndpointUrl;
            _authEndpointUrl = settings.AuthEndpointUrl ?? defaultSettings.AuthEndpointUrl;
            _authSecret = settings.AuthSecret ?? defaultSettings.AuthSecret;
            _authClientId = settings.AuthClientId ?? defaultSettings.AuthClientId;
            _scopes = settings.Scopes ?? defaultSettings.Scopes;
            _authorizeSlim = new SemaphoreSlim(1);
            _httpClient = httpClientFactory.CreateClient("ApiService");

            if (_apiEndpointUrl == null || !Uri.TryCreate(_apiEndpointUrl, UriKind.Absolute, out var _))
                throw new ArgumentException("ApiEndpoint is empty. Add appSettings section with name of this api service: " + GetType().Name);
            
            _httpClient.BaseAddress = new Uri(_apiEndpointUrl);
            _logger.LogInformation("ApiEndpoint: " + _apiEndpointUrl);
        }

        public async Task<Stream> GetStreamAsync(string url)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            return await _httpClient.GetStreamAsync(url);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            return await _httpClient.GetAsync(url);
        }

        public async Task<string> GetStringAsync(string url)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> PostAsync(string url, object data)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(url, content);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to post to {url}, statusCode: {httpResponseMessage.StatusCode}");
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task DeleteAsync(string url)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync(url);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete to {url}, statusCode: {httpResponseMessage.StatusCode}");
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_accessToken == null || DateTimeOffset.UtcNow > _accessTokenLifetime)
            {
                await Authorize();
            }
            return await _httpClient.SendAsync(request, cancellationToken);
        }

        public HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken).GetAwaiter().GetResult();
        }

        private async Task Authorize()
        {
            if (string.IsNullOrEmpty(_authEndpointUrl))
            {
                _logger.LogWarning("AuthEndpoint is null. Authentication disabled.");
                _accessToken = string.Empty;
                _accessTokenLifetime = DateTimeOffset.Now.AddHours(24.0);
                return;
            }
            try
            {
                _authorizeSlim.Wait();
                if (_accessToken == null || !(DateTimeOffset.UtcNow < _accessTokenLifetime))
                {
                    _logger.LogInformation("Authenticating at auth endpoint " + _authEndpointUrl + "...");
                    DiscoveryDocumentResponse discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(_authEndpointUrl);
                    if (discoveryDocumentResponse.IsError)
                    {
                        throw new Exception(discoveryDocumentResponse.Error);
                    }
                    TokenResponse tokenResponse = await _httpClient.RequestTokenAsync(new TokenRequest
                    {
                        Address = discoveryDocumentResponse.TokenEndpoint,
                        GrantType = "client_credentials",
                        ClientId = _authClientId,
                        ClientSecret = _authSecret,
                        ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,
                        Parameters = { { "scope", _scopes } }
                    });
                    if (tokenResponse.IsError)
                    {
                        _logger.LogError("Failed to retrieve access token, error: " + tokenResponse.Error);
                        throw new Exception(tokenResponse.Error);
                    }
                    _logger.LogInformation($"Authenticated, expires in: {tokenResponse.ExpiresIn}");
                    UpdateAuthTokens(tokenResponse);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to retrieve access token. Current token isNull: {_accessToken == null}, expires in: {_accessTokenLifetime}");
            }
            finally
            {
                _authorizeSlim.Release();
            }
        }

        private void UpdateAuthTokens(TokenResponse response)
        {
            _accessToken = response.AccessToken;
            _refreshToken = response.RefreshToken ?? _refreshToken;
            if (response.ExpiresIn > 360)
            {
                _accessTokenLifetime = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn - 360);
            }
            else if (response.ExpiresIn > 60)
            {
                _accessTokenLifetime = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn - 60);
            }
            else
            {
                _accessTokenLifetime = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn);
            }
            _httpClient.SetBearerToken(_accessToken);
        }
    }

}
