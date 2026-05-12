using System.Linq.Dynamic.Core;
using ToDoApi.DTO;

namespace ToDoApi.Extensions.QueryableExtensions;

public static class QueryableFilteringExtensions
{
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, QueryParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.Search) && !string.IsNullOrWhiteSpace(parameters.SearchBy))
        {
            return query.Where($"{parameters.SearchBy}.Contains(@0)", parameters.Search);
        }

        return query;
    }
}