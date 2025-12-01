using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Repositories.Categories;
using ToDoApi.Repositories.Products;
using ToDoApi.Repositories.Users;
using ToDoApi.Services.Auth;
using ToDoApi.Services.Categories;
using ToDoApi.Services.Products;
using ToDoApi.Services.ProductModel;
using ToDoApi.Infrastructure.Storage;
using ToDoApi.Repositories.Pages;
using MediatR;
using ToDoApi.Extensions;


namespace ToDoApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<InterfaceProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPageRepository, InMemoryPageRepository>();

            // Services
            services.AddScoped<IProdcutService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<JwtService>();
            services.AddScoped<IProductModelService, ProductModelService>();
                     
            services.AddScoped<IEmailService, SmtpEmailService>();

            // Strategies
            services.AddScoped<EmailSendStrategy>();

            //Dictionary
            services.AddScoped<IBookStore, InMemoryBookStore>();

            // Factory & Manager**
            services.AddScoped<ISendStrategyFactory, SendStrategyFactory>(); 
            services.AddScoped<INotificationManager, NotificationManager>();

            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetAllPagesHandler).Assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
