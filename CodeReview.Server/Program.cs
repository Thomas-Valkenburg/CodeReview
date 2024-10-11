using DAL;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IDbContext, Context>();

        // Add DbContext
        var connectionString = builder.Configuration.GetConnectionString("EFCoreSqlite") ??
                                    throw new InvalidOperationException("Connection string 'EFCoreSqlite' not found.");
        builder.Services.AddDbContext<Context>(options =>
        {
            options.UseSqlite(connectionString);
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            context.Database.Migrate();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
