using System.Net.Http.Json;
using ChatApp.Models;

namespace ChatApp.Client.Services;

public class UserInfoService
{
    private readonly HttpClient _client;
    private readonly Uri _baseUrl;
    
    public UserInfoService(IHttpClientFactory factory, IConfiguration configuration)
    {
        _client = factory.CreateClient();
        _baseUrl = new Uri(configuration["apiBaseUrl"]);
    }
    
    public async Task<string?> Authenticate(UserInfo user)
    {
        var requestUri = new Uri(_baseUrl, "UserInfo/authenticate");
        var response = await _client.PostAsJsonAsync(requestUri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        return null;
    }
}