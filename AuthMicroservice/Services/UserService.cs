using AuthMicroservice.Protos;
using DomainService.AuthenticationService.Input;
using Grpc.Core;
using MediatR;
using DomainService.Commands;
using DomainService.Services.AuthenticationService.Input;
namespace AuthMicroservice.Services
{
    public class UserService : Userer.UsererBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMediator _mediator;

        public UserService(ILogger<UserService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Sign up for new user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserInformation> SignUp(SignUpRequest request, ServerCallContext context)
        {
            var signUpInformation = new SignUpInput()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                UserName = request.UserName
            };
            var signUpResult = await _mediator.Send(new AddUserCommand(signUpInformation), context.CancellationToken);
            _logger.LogInformation("Signing up");
            return await Task.FromResult(new UserInformation
            {
                Email = signUpResult.Email,
                FirstName = signUpResult.FirstName,
                LastName = signUpResult.LastName,
                UserName = signUpResult.UserName
            });
        }
        /// <summary>
        /// Sign In 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SignInReply> SignIn(SignInRequest request, ServerCallContext context)
        {
            var signInInformation = new SignInInput()
            {
                Username = request.UserName,
                Password = request.Password,
                IsRememberMe = true
            };
            var signInResult = await _mediator.Send(new SignInCommand(signInInformation), context.CancellationToken);
            _logger.LogInformation("Signing in");
            return await Task.FromResult(new SignInReply
            {
                UserId = signInResult.UserId,
                UserName = signInResult.UserName,
                FirstName = signInResult.FirstName,
                LastName = signInResult.LastName,
                AccessToken = signInResult.AccessToken,
                RefreshToken = signInResult.RefreshToken
            });
        }
        /// <summary>
        /// Refresh Token for User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SignInReply> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
        {
            var refreshTokenInfor = new TokenRequest()
            {
                RefreshToken = request.RefreshToken,
                AccessToken = request.AccessToken
            };
            var refreshTokenResult = await _mediator.Send(new RefreshTokenCommand(refreshTokenInfor), context.CancellationToken);
            _logger.LogInformation("Calling Refresh Token");
            return await Task.FromResult(new SignInReply
            {
                UserId = refreshTokenResult.UserId,
                RefreshToken = refreshTokenResult.RefreshToken,
                FirstName = refreshTokenResult.FirstName,
                LastName = refreshTokenResult.LastName,
                AccessToken = refreshTokenResult.AccessToken,
                UserName = refreshTokenResult.UserName
            });
        }
        /// <summary>
        /// Sign Out
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> SignOut(Empty request, ServerCallContext context)
        {
            await _mediator.Send(new SignOutCommand(), context.CancellationToken);
            return await Task.FromResult(new Empty { });
        }
    }
}
