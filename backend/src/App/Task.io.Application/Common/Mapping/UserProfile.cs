using AutoMapper;
using Task.io.Application.Users.Commands.Create;
using Task.io.Domain.Entities;

namespace Task.io.Application.Common.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>();
    }
}
