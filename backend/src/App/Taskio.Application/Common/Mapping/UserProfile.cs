using AutoMapper;
using Taskio.Application.Users.Commands.Create;
using Taskio.Domain.Entities;

namespace Taskio.Application.Common.Mapping;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>();
    }
}
