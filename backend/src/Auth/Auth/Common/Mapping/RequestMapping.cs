using Auth.Application.Users.Commands.SignUp;
using Auth.Common.Contract.Requests.User;
using AutoMapper;

namespace Auth.Common.Mapping;

public class RequestMapping : Profile
{
    public RequestMapping()
    {
        CreateMap<LoginRequest, SignUpCommand>();
    }
}
