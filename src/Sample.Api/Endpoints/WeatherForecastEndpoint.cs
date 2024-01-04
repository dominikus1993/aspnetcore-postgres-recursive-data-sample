using System.Security.Cryptography;
using System.Text.Json.Serialization;

using FastEndpoints;

namespace Sample.Api.Endpoints;

public sealed class WeatherForecastResponse
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

[JsonSerializable(typeof(WeatherForecastResponse[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public sealed partial class WeatherForecastEndpointCtx : JsonSerializerContext { }


public sealed class WeatherForecastEndpoint : EndpointWithoutRequest<WeatherForecastResponse[]>
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public override void Configure()
    {
        Get("/weatherforecast");
        SerializerContext(WeatherForecastEndpointCtx.Default);
        Description(b => b
            .Produces<IEnumerable<WeatherForecastResponse>>()
        );
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var weathers = Enumerable.Range(1, 5).Select(index => new WeatherForecastResponse
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
                Summary = Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
            })
            .ToArray();

        if (weathers is { Length: 0})
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(weathers, cancellation: ct);
    }
}