using Microsoft.EntityFrameworkCore;

using Sample.Infrastructure.EntityFramework;

using Testcontainers.PostgreSql;

namespace Sample.Infrastructure.Tests.Fixtures;

internal sealed class CategoriesDbContextFactory : IDbContextFactory<CategoriesDbContext>
{
    private DbContextOptionsBuilder<CategoriesDbContext> _builder;

    public CategoriesDbContextFactory(string connectionString)
    {
        _builder = new DbContextOptionsBuilder<CategoriesDbContext>()
            .UseNpgsql(connectionString,
                optionsBuilder =>
                {
                    optionsBuilder.EnableRetryOnFailure(5);
                    optionsBuilder.CommandTimeout(500);
                }).UseSnakeCaseNamingConvention();
    }
    public CategoriesDbContext CreateDbContext()
    {
        return new CategoriesDbContext(_builder.Options);
    }
}

public sealed class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer = new PostgreSqlBuilder().Build();
    
    internal CategoriesDbContext CategoriesDbContext { get; private set; } = null!;
    internal CategoriesDbContextFactory CategoriesDbContextFactory { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();
        CategoriesDbContextFactory = new CategoriesDbContextFactory(PostgreSqlContainer.GetConnectionString());
        CategoriesDbContext = CategoriesDbContextFactory.CreateDbContext();
        await CategoriesDbContext.Database.MigrateAsync();
    }
    
    public void ClearDatabaseAsync()
    {
        CategoriesDbContext.Categories.RemoveRange(CategoriesDbContext.Categories);
        CategoriesDbContext.SaveChanges();
    }
    
    public async Task DisposeAsync()
    {
        await CategoriesDbContext.DisposeAsync();
        await PostgreSqlContainer.DisposeAsync();
    }
}