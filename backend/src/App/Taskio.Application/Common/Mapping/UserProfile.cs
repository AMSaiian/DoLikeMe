using AutoMapper;
using Taskio.Application.Users.Commands.Create;
using Taskio.Domain.Entities;

namespace Taskio.Application.Common.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>();
    }
}
