using Auth.Application.Common.Models.User;
using Auth.Application.Users.Commands.ChangePassword;
using Auth.Application.Users.Commands.Delete;
using Auth.Application.Users.Commands.Register;
using Auth.Application.Users.Commands.SignUp;
using Auth.Application.Users.Commands.UpdateProfileInfo;
using AutoMapper;

namespace Auth.Application.Common.Mapping;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, UserDto>();

        CreateMap<UpdateProfileInfoCommand, UpdateUserProfileDto>();

        CreateMap<ChangePasswordCommand, UpdateUserPasswordDto>();

        CreateMap<SignUpCommand, SignUpDto>();

        CreateMap<DeleteUserCommand, DeleteUserDto>();
    }
}
