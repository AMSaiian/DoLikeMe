using Auth.Application.Users.Commands.Delete;
using Auth.Application.Users.Commands.Register;
using AutoMapper;
using Taskio.Common.Contract.Requests.User;

namespace Taskio.Common.Mapping;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, RegisterUserCommand>();

        CreateMap<DeleteUserRequest, DeleteUserCommand>();
    }
}
