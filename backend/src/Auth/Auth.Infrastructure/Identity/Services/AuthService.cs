using System.Security.Claims;
using AMSaiian.Shared.Application.Exceptions;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;
using Auth.Infrastructure.Common.Interfaces;
using Auth.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnauthorizedAccessException = AMSaiian.Shared.Application.Exceptions.UnauthorizedAccessException;

namespace Auth.Infrastructure.Identity.Services;

public class AuthService(ITokenProvider tokenProvider,
                         UserManager<AuthUser> userManager,
                         IUserClaimsPrincipalFactory<AuthUser> claimsFactory)
    : IAuthService
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IUserClaimsPrincipalFactory<AuthUser> _claimsFactory = claimsFactory;
    private readonly UserManager<AuthUser> _userManager = userManager;

    public async Task<Guid> CreateUser(UserDto user, CancellationToken cancellationToken = default)
    {
        if (await IsUserNameExists(user.Name, cancellationToken)
         || await IsEmailExists(user.Email, cancellationToken))
        {
            throw new ConflictException(ErrorMessagesConstants.IdentifierDataNotUnique);
        }

        var newUser = new AuthUser
        {
            UserName = user.Name,
            Email = user.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

        if (!result.Succeeded)
        {
            throw new UnprocessableException(string.Concat(
                                                 result.Errors
                                                     .Select(e => e.Description),
                                                 "\n"));
        }

        if (!result.Succeeded)
        {
            throw new UnprocessableException(string.Concat(
                                                 result.Errors
                                                     .Select(e => e.Description),
                                                 "\n"));
        }

        return newUser.Id;
    }

    public async Task UpdateUser(UpdateUserProfileDto user, CancellationToken cancellationToken = default)
    {
        if (await IsUserNameExists(user.NewName, cancellationToken)
         || await IsEmailExists(user.NewEmail, cancellationToken))
        {
            throw new ConflictException(ErrorMessagesConstants.IdentifierDataNotUnique);
        }

        AuthUser updatingUser = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == user.Id,
                                  cancellationToken)
                             ?? throw new NotFoundException(
                                    string.Format(ErrorMessagesConstants.UserNotFound,
                                                  user.Id));

        if (user.NewEmail is not null)
        {
            updatingUser.Email = user.NewEmail;
        }

        if (user.NewName is not null)
        {
            updatingUser.UserName = user.NewName;
        }

        IdentityResult result = await _userManager.UpdateAsync(updatingUser);

        if (!result.Succeeded)
        {
            throw new UnprocessableException(string.Concat(
                                                 result.Errors
                                                     .Select(e => e.Description),
                                                 "\n"));
        }
    }

    public async Task ChangePassword(UpdateUserPasswordDto user, CancellationToken cancellationToken = default)
    {
        AuthUser updatingUser = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == user.Id,
                                  cancellationToken)
                             ?? throw new NotFoundException(
                                    string.Format(ErrorMessagesConstants.UserNotFound,
                                                  user.Id));

        IdentityResult result = await _userManager.ChangePasswordAsync(updatingUser,
                                                                       user.OldPassword,
                                                                       user.NewPassword);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(error => error.Code == "PasswordMismatch"))
            {
                throw new UnauthorizedAccessException(string.Concat(
                                                          result.Errors
                                                              .Select(e => e.Description),
                                                          "\n"));
            }

            throw new UnprocessableException(string.Concat(
                                                 result.Errors
                                                     .Select(e => e.Description),
                                                 "\n"));
        }
    }

    public async Task<string> SignUpUser(SignUpDto user, CancellationToken cancellationToken = default)
    {
        AuthUser signingUpUser = await _userManager.FindByEmailAsync(user.Identifier)
                              ?? await _userManager.FindByNameAsync(user.Identifier)
                              ?? throw new NotFoundException(
                                     string.Format(ErrorMessagesConstants.UserNotFound,
                                                   user.Identifier));

        bool result = await _userManager.CheckPasswordAsync(signingUpUser, user.Password);

        if (!result)
        {
            throw new UnauthorizedAccessException(ErrorMessagesConstants.InvalidPassword);
        }

        ClaimsPrincipal claims = await _claimsFactory.CreateAsync(signingUpUser);
        string token = await _tokenProvider.CreateToken(claims, cancellationToken);

        return token;
    }

    public async Task DeleteUser(DeleteUserDto user, CancellationToken cancellationToken = default)
    {
        AuthUser deletingUser = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id,
                                 cancellationToken)
                             ?? throw new NotFoundException(
                                    string.Format(ErrorMessagesConstants.UserNotFound,
                                                  user.Id));

        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(deletingUser, user.Password);

        if (!isPasswordCorrect)
        {
            throw new UnauthorizedAccessException(ErrorMessagesConstants.InvalidPassword);
        }

        IdentityResult result = await _userManager.DeleteAsync(deletingUser);

        if (!result.Succeeded)
        {
            throw new UnprocessableException(string.Concat(
                                                 result.Errors
                                                     .Select(e => e.Description),
                                                 "\n"));
        }
    }

    private Task<bool> IsUserNameExists(string? userName, CancellationToken cancellationToken = default)
        => _userManager.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

    private Task<bool> IsEmailExists(string? email, CancellationToken cancellationToken = default)
        => _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);


}
