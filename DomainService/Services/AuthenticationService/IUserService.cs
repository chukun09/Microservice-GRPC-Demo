using Core.Entites;
using DomainService.AuthenticationService.Input;
using DomainService.Services.AuthenticationService.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services.AuthenticationService
{
    public interface IUserService
    {
        //Task CreateAsync(CreateUserInput input);
        //Task<Input.SignInResult> GenerateJwtToken(UserEntity user, UserInfoApi userInfo);
        Task<SignInResult> RefreshTokenAsync(TokenRequest input);
        Task<SignInResult> SignInAsync(SignInInput input);
        Task<UserEntity> SignUpAsync(SignUpInput input, CancellationToken ct);
        Task SignOut();
    }
}
