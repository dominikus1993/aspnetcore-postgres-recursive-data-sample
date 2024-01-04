using Microsoft.Extensions.DependencyInjection;

namespace Sample.Core.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}