using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Behaviors;
using ToDoApi.Repositories.Categories;
using ToDoApi.Repositories.Products;
using ToDoApi.Repositories.Users;
using ToDoApi.Services.Auth;
using ToDoApi.Services.Categories;
using ToDoApi.Services.Products;
using ToDoApi.Services.ProductModel;
using ToDoApi.Infrastructure.Storage;

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

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<JwtService>();
            services.AddScoped<IProductModelService, ProductModelService>();
                     
            services.AddScoped<IEmailService, SmtpEmailService>();

            // Strategies
            services.AddScoped<EmailSendStrategy>();

            //Dictionary
            services.AddScoped<IBookStore, InMemoryBookStore>();

            // **Factory & Manager**
            services.AddScoped<ISendStrategyFactory, SendStrategyFactory>(); // <--- Missing line
            services.AddScoped<INotificationManager, NotificationManager>();

            services.AddErrorHandling();

            services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.AddMediatR(cfg =>
            {
               //Mediator
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            return services;
        }
    }
}
