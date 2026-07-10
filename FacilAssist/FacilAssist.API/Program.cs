using FacilAssist.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<FacilAssist.API.Services.IClienteService, FacilAssist.API.Services.ClienteService>();
builder.Services.AddScoped<FacilAssist.API.Repositories.IClienteRepository, FacilAssist.API.Repositories.ClienteRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
