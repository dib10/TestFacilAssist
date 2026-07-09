var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<FacilAssist.API.Services.IClienteService, FacilAssist.API.Services.ClienteService>();
builder.Services.AddScoped<FacilAssist.API.Repositories.IClienteRepository, FacilAssist.API.Repositories.ClienteRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
