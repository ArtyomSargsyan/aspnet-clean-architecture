using System.Linq.Dynamic.Core;
using ToDoApi.DTO;

namespace ToDoApi.Extensions.QueryableExtensions;

public static class QueryableSortingExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, QueryParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            var direction = parameters.IsDescending ? "descending" : "ascending";
            return query.OrderBy($"{parameters.SortBy} {direction}");
        }

        return query;
    }
}