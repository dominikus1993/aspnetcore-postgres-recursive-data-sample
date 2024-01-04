using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sample.Infrastructure.EntityFramework;

namespace Sample.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<CategoriesDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("CategoriesDb"), optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure(2);
            }).UseSnakeCaseNamingConvention();
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        return services;
    }
}