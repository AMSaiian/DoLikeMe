using Auth.Application.Users.Commands.Register;
using AutoMapper;
using Task.io.Common.Contract.Requests.User;

namespace Task.io.Common.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<CreateUserRequest, RegisterUserCommand>();
    }
}
