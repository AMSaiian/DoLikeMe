using Auth.Application.Users.Commands.Register;
using AutoMapper;
using Taskio.Common.Contract.Requests.User;

namespace Taskio.Common.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<CreateUserRequest, RegisterUserCommand>();
    }
}
