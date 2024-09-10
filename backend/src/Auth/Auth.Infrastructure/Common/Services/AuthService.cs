using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;

namespace Auth.Infrastructure.Common.Services;

public class AuthService : IAuthService
{
    public Task<Guid> CreateUser(UserDto user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(UpdateUserProfileDto user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ChangePassword(UpdateUserPasswordDto user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> SignUpUser(SignUpDto user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
