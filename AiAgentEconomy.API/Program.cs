using AiAgentEconomy.Infrastructure.DependencyInjection;
using AiAgentEconomy.Infrastructure.Persistence;
using AiAgentEconomy.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger (Swashbuckle)
builder.Services.AddSwaggerGen();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed on startup (dev-friendly)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AgentEconomyDbContext>();
    await SeedData.ApplyAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();