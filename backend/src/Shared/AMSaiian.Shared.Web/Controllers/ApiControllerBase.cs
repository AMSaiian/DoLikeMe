using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase(
    ISender mediator,
    IMapper mapper)
    : ControllerBase
{
    protected readonly ISender _mediator = mediator;
    protected readonly IMapper _mapper = mapper;
}
