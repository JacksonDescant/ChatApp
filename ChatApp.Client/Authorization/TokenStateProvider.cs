using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Text;
using ChatApp.Client.Services;
using ChatApp.Models;

namespace ChatApp.Client.Authorization;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
{
    /// <summary>
    /// Map the JavaScript-formatted properties to C#-formatted classes.
    /// </summary>
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

    /// <summary>
    /// Special auth client.
    /// </summary>
    private readonly HttpClient _httpClient;
    private readonly UserInfoService _userInfoService;
    
    private string _token = string.Empty;

    /// <summary>
    /// Authentication state.
    /// </summary>
    private bool _authenticated = false;

    /// <summary>
    /// Default principal for anonymous (not authenticated) users.
    /// </summary>
    private readonly ClaimsPrincipal Unauthenticated =
        new(new ClaimsIdentity());

    /// <summary>
    /// Create a new instance of the auth provider.
    /// </summary>
    /// <param name="httpClientFactory">Factory to retrieve auth client.</param>
    public TokenAuthenticationStateProvider(IHttpClientFactory httpClientFactory, UserInfoService userInfoService)
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
        _userInfoService = userInfoService;
    }


    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The result serialized to a <see cref="FormResult"/>.
    /// </returns>
    public async Task RegisterAsync(string email, string password)
    {
    }

    /// <summary>
    /// User login.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
    public async Task LoginAsync(string username, string password)
    {
        var token = await _userInfoService.Authenticate(new UserInfo
        {
            Username = username,
            Password = password
        });
        //store token somewhere
        Console.WriteLine(token); 
        _token = token;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    /// <summary>
    /// Get authentication state.
    /// </summary>
    /// <remarks>
    /// Called by Blazor anytime and authentication-based decision needs to be made, then cached
    /// until the changed state notification is raised.
    /// </remarks>
    /// <returns>The authentication state asynchronous request.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _authenticated = false;

        // default to not authenticated
        var user = Unauthenticated;
        
        //get the stored token and return an authenticated state

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(_token) as JwtSecurityToken;

            if (tokenS != null)
            {
                var userInfo = new UserInfo
                {
                    Email = tokenS.Claims.First(claim => claim.Type == "email").Value,
                    Username = tokenS.Claims.First(claim => claim.Type == "username").Value
                };
                
                // in our system name and email are the same
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, userInfo.Username),
                    new(ClaimTypes.Email, userInfo.Email)
                };
                
                // set the principal
                var id = new ClaimsIdentity(claims, nameof(TokenAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
                _authenticated = true;
            }
        }
        catch
        {
        }

        // return the state
        return new AuthenticationState(user);
    }

    public async Task LogoutAsync()
    {
        _token = string.Empty;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _authenticated;
    }

    public class RoleClaim
    {
        public string? Issuer { get; set; }
        public string? OriginalIssuer { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
    }
}