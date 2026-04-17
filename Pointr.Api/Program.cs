using Pointr.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ISiteRepository, InMemorySiteRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program;
