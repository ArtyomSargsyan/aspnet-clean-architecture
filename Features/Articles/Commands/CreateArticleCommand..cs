using MediatR;

namespace ToDoApi.Features.Articles.Commands;

public record CreateArticleCommand(string Title, string Content) : IRequest<int>;