using Pointr.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ISiteRepository, InMemorySiteRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();

/// <summary>Integration testlerin WebApplicationFactory ile API'yi başlatabilmesi için açılan partial Program class'ıdır.</summary>
public partial class Program;
