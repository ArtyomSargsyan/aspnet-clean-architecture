using MediatR;
using ToDoApi.DTO;
namespace ToDoApi.Repositories.Pages;
using AutoMapper;

using GetAllPagesQuery = TODOAPI.CQRS.Pages.Queries.GetAllPagesQuery;

public class GetAllPagesHandler 
    : IRequestHandler<GetAllPagesQuery, List<PageDto>>
{
    private readonly IPageRepository _repo;
    private readonly IMapper _mapper;

    public GetAllPagesHandler(IPageRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<PageDto>> Handle(
        GetAllPagesQuery request,
        CancellationToken cancellationToken)
    {
        var pages = await _repo.GetAllAsync();
        return _mapper.Map<List<PageDto>>(pages);
    }
}
