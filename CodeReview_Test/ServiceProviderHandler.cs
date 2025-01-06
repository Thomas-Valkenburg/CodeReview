using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.Server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test;

internal class ServiceProviderHandler
{
    private static ServiceProvider? _serviceProvider;
    private static IServiceScope? _scope;

    internal static IServiceScope GetScope()
    {
        if (_serviceProvider is null) CreateServiceProvider();

        while (_serviceProvider is null) { }

        return _scope ??= _serviceProvider.CreateScope();
    }

    private static void CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<Context>(options =>
        {
            options.UseInMemoryDatabase("TestDb");
            options.EnableSensitiveDataLogging();
		});

        services.AddDbContext<AccountContext>(options =>
        {
            options.UseInMemoryDatabase("AccountTestDb");
			options.EnableSensitiveDataLogging();
		});

        services.AddScoped<IUserService, DAL.Services.UserService>();
        services.AddScoped<IPostService, DAL.Services.PostService>();
        services.AddScoped<ICommentService, DAL.Services.CommentService>();

        services.AddTransient<UserHandler>();
		services.AddTransient<PostHandler>();
		services.AddTransient<CommentHandler>();

		services.AddTransient<AccountController>();
        services.AddTransient<CommentController>();
		services.AddTransient<PostController>();
        services.AddTransient<UserController>();

        services.AddScoped<TestWebApplicationFactory>();

        services.AddAuthorization();
        services.AddControllers();

		_serviceProvider = services.BuildServiceProvider();
    }
}