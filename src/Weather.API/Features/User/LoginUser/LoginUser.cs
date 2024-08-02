using API.Auth;
using API.Common.Exceptions;
using API.Common.Utilities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.User.LoginUser
{
    public class LoginUser
    {
        public record LoginUserCommand(string UserName, string Password) : IRequest<LoginUserResponse>;

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
        {
            private readonly ITokenService _tokenService;
            private readonly DataContext _context;

            public LoginUserCommandHandler(ITokenService tokenService, DataContext context)
            {
                _tokenService = tokenService;
                _context = context;
            }

            public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

                if (user == null || !PasswordHasher.VerifyHashedPassword(user.Password, request.Password))
                {
                    throw new BusinessException("Invalid username or password");
                }

                return new LoginUserResponse
                {
                    AccessToken = _tokenService.GenerateBearerToken(user)
                };
            }
        }
    }    
}
