using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using WebAppBlazor.Services.Authentication;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken);
}

public class AppTokenProvider : ITokenProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthService _authService;
    public AppTokenProvider(ILocalStorageService localStorage, IAuthService authService)
    {
        _localStorage = localStorage;
        _authService = authService;
    }

    private string _token;

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        if (_token == null)
        {
            _token = await _localStorage.GetItemAsync<string>("accessToken") ?? string.Empty;
        }

        if (!string.IsNullOrEmpty(_token))
        {
            // Check if the token is expired
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(_token) as JwtSecurityToken;

            if (token?.ValidTo < DateTime.UtcNow)
            {
                try
                {
                    // Token is expired, refresh token
                    _token = await _authService.RefreshToken();
                    return _token;
                }
                catch (Exception)
                {
                    await _authService.Logout();
                }
            }
            else return _token;
        }
        await _authService.Logout();

        return _token;
    }
}