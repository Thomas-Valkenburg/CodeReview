﻿using Microsoft.EntityFrameworkCore;

namespace CodeReview.Core.Interfaces;

public interface IDbContext : IDisposable
{
	public DbSet<TEntity> Set<TEntity>() where TEntity : class;

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}