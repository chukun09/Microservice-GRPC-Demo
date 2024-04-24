using Core.Entites;
using DomainService.AuthenticationService.Input;
using DomainService.Commands;
using DomainService.Events.Notifications;
using DomainService.Services.AuthenticationService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Handler
{
    public class SignUpHandler : IRequestHandler<AddUserCommand, UserEntity>
    {
        protected readonly IMediator _mediator;
        protected readonly IUserService _userService;

        public SignUpHandler(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        public async Task<UserEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.SignUpAsync(request.input, cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Sign In Handler
    /// </summary>
    public class SignInHandler : IRequestHandler<SignInCommand, SignInResult>
    {
        protected readonly IMediator _mediator;
        protected readonly IUserService _userService;

        public SignInHandler(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        public async Task<SignInResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.SignInAsync(request.input);
            await _mediator.Publish(new UserLoggedinNotification(result), cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// RefreshToken Handler
    /// </summary>
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, SignInResult>
    {
        protected readonly IMediator _mediator;
        protected readonly IUserService _userService;

        public RefreshTokenHandler(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        public async Task<SignInResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.RefreshTokenAsync(request.input);
            return result;
        }
    }
    /// <summary>
    /// Signout Handler
    /// </summary>
    public class SignOutHandler : IRequestHandler<SignOutCommand>
    {
        protected readonly IUserService _userService;

        public SignOutHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            await _userService.SignOut();
        }
    }
}
