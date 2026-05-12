using MediatR;
using ToDoApi.Models;

namespace ToDoApi.Features.Articles.Queries;

public record GetAllArticlesQuery() : IRequest<List<Article>>;