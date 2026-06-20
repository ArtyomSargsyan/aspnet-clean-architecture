using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Endpoints;

namespace ToDoApi.Extensions;

public static class EndpointExtensions
{
    public static IApplicationBuilder MapAllEndpoints(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var endpoint = (IEndpoint)ActivatorUtilities.CreateInstance(app.Services, type);
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}