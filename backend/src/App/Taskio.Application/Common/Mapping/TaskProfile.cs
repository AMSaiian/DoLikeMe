using AutoMapper;
using Taskio.Application.Common.Models.Task;
using Taskio.Application.Tasks.Commands.Create;
using Taskio.Application.Tasks.Commands.Update;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Application.Common.Mapping;

public sealed class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<CreateTaskCommand, Task>();

        CreateMap<UpdateTaskCommand, Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title is not null))
            .ForMember(dest => dest.Description, opt =>
            {
                opt.MapFrom(src => src.Description.Value);
                opt.Condition(src => src.Description.IsDefined);
            })
            .ForMember(dest => dest.DueDate, opt =>
            {
                opt.MapFrom(src => src.DueDate.Value);
                opt.Condition(src => src.DueDate.IsDefined);
            })
            .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status is not null))
            .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority is not null));

        CreateMap<Task, TaskShortDto>()
            .IncludeAllDerived();

        CreateMap<Task, TaskFullDto>();
    }
}
