using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces;

public interface IDbContext : IDisposable
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}