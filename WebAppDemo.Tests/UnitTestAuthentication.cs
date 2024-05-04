using Core.Entites;
using DomainService.AuthenticationService.Input;
using DomainService.Services.AuthenticationService;
using DomainService.Services.AuthenticationService.Input;
using Microsoft.AspNetCore.Http;

namespace WebAppDemo.Tests
{
    public class UnitTestAuthentication
    {
        private readonly IUserService _userService;

        public UnitTestAuthentication(IUserService userService)
        {
            _userService = userService;
        }
        [Fact]
        public async Task SignIn_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var signInInput = new SignInInput()
            {
                IsRememberMe = true,
                Password = "Demo123@@",
                Username = "demo001"
            };


            // Act                   
            var result = await _userService.SignInAsync(signInInput);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SignInResult>(result);
            // Add more assertions if needed
        }

        [Fact]
        public async Task SignUp_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var signUpInput = new SignUpInput()
            {
                Email = "demo@gmail.com",
                FirstName = "Unit",
                LastName = "Test",
                Password = "UnitTest123@@",
                UserName = "unittest123"
            };

            // Act
            var result = await _userService.SignUpAsync(signUpInput, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserEntity>(result);
            // Add more assertions if needed
        }

        //[Fact]
        //public async Task RefreshToken_ValidRefreshToken_ReturnsNewToken()
        //{
        //    // Arrange
        //    var refreshToken = "valid_refresh_token";

        //    // Set up expected behavior for RefreshToken method
        //    _userServiceMock.Setup(x => x.RefreshToken(refreshToken))
        //                    .ReturnsAsync("new_access_token");

        //    var userService = _userServiceMock.Object;

        //    // Act
        //    var newAccessToken = await userService.RefreshToken(refreshToken);

        //    // Assert
        //    Assert.NotNull(newAccessToken);
        //    // Add more assertions if needed
        //}
    }
}
