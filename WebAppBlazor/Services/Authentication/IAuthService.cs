
using DomainService.AuthenticationService.Input;
using DomainService.Services.AuthenticationService.Input;

namespace WebAppBlazor.Services.Authentication
{
    public interface IAuthService
    {
        Task<SignInResult> Login(SignInInput input);
        Task Logout();
        Task Register(SignUpInput input);
        Task<string> RefreshToken();
    }
}
