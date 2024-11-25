using Core.Interfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview_Test;

internal static class ServiceProviderHandler
{
    private static ServiceProvider? _serviceProvider;
    private static IServiceScope? _scope;

    internal static IServiceScope GetScope()
    {
        while (_serviceProvider is null) CreateServiceProvider();

        return _scope ??= _serviceProvider.CreateScope();
    }

    private static void CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<Context>(options =>
        {
            options.UseInMemoryDatabase("TestDb");
        });

        services.AddScoped<IDbContext, Context>();

        services.AddScoped<IUserService, DAL.Services.UserService>();
        services.AddScoped<IPostService, DAL.Services.PostService>();
        services.AddScoped<ICommentService, DAL.Services.CommentService>();

        _serviceProvider = services.BuildServiceProvider();
    }
}