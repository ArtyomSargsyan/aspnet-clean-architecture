using MediatR;
using ToDoApi.DTO;
namespace TODOAPI.CQRS.Pages.Queries;
public record GetAllPagesQuery() : IRequest<List<PageDto>>;
