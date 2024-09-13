using AutoMapper;
using Taskio.Application.Tasks.Commands.Create;
using Taskio.Application.Tasks.Commands.Update;
using Taskio.Common.Contract.Requests.Task;

namespace Taskio.Common.Mapping;

public class TaskMapping : Profile
{
    public TaskMapping()
    {
        CreateMap<CreateTaskRequest, CreateTaskCommand>();

        CreateMap<UpdateTaskRequest, UpdateTaskCommand>();
    }
}
