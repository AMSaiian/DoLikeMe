using Auth.Application.Users.Commands.ChangePassword;
using Auth.Application.Users.Commands.SignUp;
using Auth.Application.Users.Commands.UpdateProfileInfo;
using Auth.Common.Contract.Requests.User;
using AutoMapper;

namespace Auth.Common.Mapping;

public sealed class RequestMapping : Profile
{
    public RequestMapping()
    {
        CreateMap<LoginRequest, SignUpCommand>();

        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();

        CreateMap<UpdateIdentifierRequest, UpdateProfileInfoCommand>();
    }
}
