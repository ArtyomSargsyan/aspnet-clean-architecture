using Microsoft.AspNetCore.Routing;

namespace ToDoApi.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

