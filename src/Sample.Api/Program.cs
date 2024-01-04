
using System.Diagnostics;
using System.Text.Json.Serialization;

using Sample.Infrastructure.Extensions;
using Sample.Core.Extensions;

using FastEndpoints;
using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Http.Json;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Activity.ForceDefaultIdFormat = true;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddFastEndpoints();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors();

var app = builder.Build();

app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen(); //add this


app.Run();

public partial class Program
{
}
