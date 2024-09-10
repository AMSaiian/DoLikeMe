using Auth.Application.Common.Models.User;
using Auth.Application.Users.Commands.ChangePassword;
using Auth.Application.Users.Commands.Register;
using Auth.Application.Users.Commands.UpdateProfileInfo;
using AutoMapper;

namespace Auth.Application.Common.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, UserDto>();

        CreateMap<UpdateProfileInfoCommand, UpdateUserProfileDto>();

        CreateMap<ChangePasswordCommand, UpdateUserPasswordDto>();
    }
}
