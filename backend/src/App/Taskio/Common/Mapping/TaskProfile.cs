using AutoMapper;
using Taskio.Application.Tasks.Commands.Create;
using Taskio.Application.Tasks.Commands.Update;
using Taskio.Common.Contract.Requests.Task;

namespace Taskio.Common.Mapping;

public sealed class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<CreateTaskRequest, CreateTaskCommand>();

        CreateMap<UpdateTaskRequest, UpdateTaskCommand>();
    }
}
