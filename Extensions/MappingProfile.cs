using AutoMapper;
using ToDoApi.DTO;
using ToDoApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Page, PageDto>();
    }
}
