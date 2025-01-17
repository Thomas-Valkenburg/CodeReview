using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.WebSockets;

namespace CodeReview.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<ICommentService, CommentService>();

        builder.Services.AddTransient<UserHandler>();
        builder.Services.AddTransient<PostHandler>();
        builder.Services.AddTransient<CommentHandler>();

		// Add DbContext(s)
		var connectionString = builder.Configuration.GetConnectionString("EFCoreSqlite") ??
                               throw new InvalidOperationException("Connection string 'EFCoreSqlite' not found.");
        builder.Services.AddDbContext<Context>(options => { options.UseSqlite(connectionString); });

        var accountConnectionString = builder.Configuration.GetConnectionString("EFCoreAccountSqlite") ??
                                      throw new InvalidOperationException(
                                          "Connection string 'EFCoreAccountSqlite' not found.");
        builder.Services.AddDbContext<AccountContext>(options => { options.UseSqlite(accountConnectionString); });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Identity Framework", new OpenApiSecurityScheme
            {
                Name = "Login",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Cookie
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    []
                }
            });
        });

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        builder.Services.AddWebSockets(options =>
        {
            options.AllowedOrigins.Add("*");
            options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            options.KeepAliveTimeout = TimeSpan.FromSeconds(5);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", policy =>
            {
#if DEBUG
                policy.WithOrigins("http://localhost:5173", "https://localhost:5173").AllowAnyHeader().AllowAnyMethod();
#endif
            });
        });

		builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail    = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail         = true;
            options.Password.RequiredLength         = 8;
            options.Password.RequireUppercase       = true;
        }).AddEntityFrameworkStores<AccountContext>();

        var app = builder.Build();

        // Migrate databases
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

            if (context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
            else
            {
                context.Database.EnsureCreated();
            }

            if (accountContext.Database.IsRelational())
            {
                accountContext.Database.Migrate();
            }
            else
            {
                accountContext.Database.EnsureCreated();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapSwagger().RequireAuthorization();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapIdentityApi<IdentityUser>();

        app.MapControllers();

        app.UseWebSockets();

        app.UseCors("AllowedOrigins");

        app.Run();
    }
}