using MyApi.Api.Extensions;
using MyApi.Application.Extensions;
using MyApi.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistenceServices(builder.Configuration); // DbContext ve repository
builder.Services.AddApplicationServices();                        // Service�ler (repository inject edebilir)
builder.Services.AddApiValidators();                              // Validator pipeline (controller/service i�in)

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
