using MediatR;

namespace ToDoApi.Features.Articles.Commands;

public record DeleteArticleCommand(int Id) : IRequest<Unit>;
