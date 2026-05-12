using MediatR;
using ToDoApi.Models;

namespace ToDoApi.Features.Articles.Queries;

public record GetArticleByIdQuery(int Id) : IRequest<Article>;
