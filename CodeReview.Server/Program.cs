using DAL;
using DAL_Account;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CodeReview.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IDbContext, Context>();
        builder.Services.AddScoped<AccountContext>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<ICommentService, CommentService>();

        builder.Services.AddTransient<UserHandler>();
        builder.Services.AddTransient<PostHandler>();

        // Add DbContext(s)
        var connectionString = builder.Configuration.GetConnectionString("EFCoreSqlite") ??
                               throw new InvalidOperationException("Connection string 'EFCoreSqlite' not found.");
        builder.Services.AddDbContext<Context>(options => { options.UseSqlite(connectionString); });

        var accountConnectionString = builder.Configuration.GetConnectionString("EFCoreAccountSqlite") ??
                                      throw new InvalidOperationException(
                                          "Connection string 'EFCoreAccountSqlite' not found.");

        builder.Services.AddDbContext<AccountContext>(options => { options.UseSqlite(accountConnectionString); });

        builder.Services.AddControllers();
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

        builder.Services.AddIdentityApiEndpoints<AccountUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AccountContext>();

        var app = builder.Build();

        // Migrate databases
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

            context.Database.Migrate();
            accountContext.Database.Migrate();
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

        app.MapIdentityApi<AccountUser>();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}