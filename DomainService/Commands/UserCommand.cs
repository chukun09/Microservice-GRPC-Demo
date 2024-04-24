using Core.Entites;
using DomainService.AuthenticationService.Input;
using DomainService.Services.AuthenticationService.Input;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Commands
{
        public record AddUserCommand(SignUpInput input) : IRequest<UserEntity>;
        public record SignInCommand(SignInInput input) : IRequest<SignInResult>;
        public record RefreshTokenCommand(TokenRequest input) : IRequest<SignInResult>;
        public record SignOutCommand() : IRequest;
}
