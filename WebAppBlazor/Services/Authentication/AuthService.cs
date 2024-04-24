using AuthenticationWithClientSideBlazor.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using DomainService.Services.AuthenticationService.Input;
using Core.Entites;
using DomainService.AuthenticationService.Input;
using AuthMicroservice.Protos;

namespace WebAppBlazor.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly Userer.UsererClient _userClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage, Userer.UsererClient userClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _userClient = userClient;
        }

        public async Task Register(SignUpInput registerModel)
        {
            var input = new SignUpRequest()
            {
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Password = registerModel.Password,
                UserName = registerModel.UserName,
            };
            _ = await _userClient.SignUpAsync(input);
            //return response;
            //var result = await _httpClient.PostJsonAsync<UserEntity>("api/accounts", registerModel);
        }

        public async Task<SignInResult> Login(SignInInput loginModel)
        {
            var input = new SignInRequest()
            {
                Password = loginModel.Password,
                UserName = loginModel.Username
            };
            var response = await _userClient.SignInAsync(input);

            await _localStorage.SetItemAsync("accessToken", response.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", response.RefreshToken);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.AccessToken);

            var result = new SignInResult()
            {
                AccessToken = response.AccessToken,
                UserName = response.UserName,
                FirstName = response.FirstName,
                LastName = response.LastName,
                RefreshToken = response.RefreshToken,
                UserId = response.UserId
            };
            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            //_httpClient.DefaultRequestHeaders.Authorization = null;
            var empty = new Empty();
            await _userClient.SignOutAsync(empty);
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("accessToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var tokenDto = new RefreshTokenRequest() { AccessToken = token, RefreshToken = refreshToken };
            var refreshResult = await _userClient.RefreshTokenAsync(tokenDto);
            await _localStorage.SetItemAsync("accessToken", refreshResult.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", refreshResult.RefreshToken);
            return refreshResult.AccessToken;
        }
    }
}
