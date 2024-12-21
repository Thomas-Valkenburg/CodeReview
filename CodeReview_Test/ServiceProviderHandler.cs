using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test;

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

        services.AddScoped<IUserService, global::CodeReview.DAL.Services.UserService>();
        services.AddScoped<IPostService, global::CodeReview.DAL.Services.PostService>();
        services.AddScoped<ICommentService, global::CodeReview.DAL.Services.CommentService>();

        services.AddTransient<UserHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }
}