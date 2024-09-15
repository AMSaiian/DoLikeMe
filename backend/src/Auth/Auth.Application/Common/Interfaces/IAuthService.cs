using Auth.Application.Common.Models.Token;
using Auth.Application.Common.Models.User;

namespace Auth.Application.Common.Interfaces;

public interface IAuthService
{
    public Task<Guid> CreateUser(UserDto user, CancellationToken cancellationToken = default);

    public Task UpdateUser(UpdateUserProfileDto user, CancellationToken cancellationToken = default);

    public Task ChangePassword(UpdateUserPasswordDto user, CancellationToken cancellationToken = default);

    public Task<TokenDto> SignUpUser(SignUpDto user, CancellationToken cancellationToken = default);

    public Task DeleteUser(DeleteUserDto user, CancellationToken cancellationToken = default);
}
