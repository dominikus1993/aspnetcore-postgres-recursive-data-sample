using Alba;

using Microsoft.Extensions.Configuration;

namespace Sample.Api.Tests.Fixtures;

public class AspNetCoreFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; }
    
    
    public void Dispose()
    {
    }

    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>(host =>
        {

            host.ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("./Api/appsettings.json", optional: false, reloadOnChange: true);
                var dict = new Dictionary<string, string>
                {
                };
                builder.AddInMemoryCollection(dict!);
            });
        });
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }
}