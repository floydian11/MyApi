using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Api.Extensions;
using MyApi.Api.Middlewares;
using MyApi.Application.Extensions;
using MyApi.Application.Mapping;
using MyApi.Persistence.Extensions;
using Serilog;
using Serilog.Events;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

//SERILOG CONFIG
// 1) Serilog konfigurasyonu (Program.cs'te olmalı)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(builder.Environment.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Error)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("App", "MyApi")           // istersen app/tenant bilgisi ekle
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")          // Seq URL  
    .CreateLogger();

builder.Host.UseSerilog();

// Common infra/application services
builder.Services.AddHttpContextAccessor();  // audit için gerekli olacak

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CategoryProfile>();

    // diğer profiller...
});           // mapping profilleri (Application katmanındaki)

builder.Services.AddPersistenceServices(builder.Configuration); // DbContext ve repository
builder.Services.AddApplicationServices();                        // Service’ler (repository inject edebilir)
builder.Services.AddApiValidators();                              // Validator pipeline (controller/service için)


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

try
{
    // 4) Pipeline: Exception middleware en başta olmalı
    app.UseMiddleware<ExceptionMiddleware>();   // bizim özel ExceptionMiddleware

    // isteğe bağlı: istersen HTTP request loglarını otomatik almak için
    // app.UseSerilogRequestLogging(); // çok fazla request logu istemiyorsan kapalı bırak

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors("AllowAll"); // eğer policy tanımlandıysa
    app.UseAuthentication();
    app.UseAuthorization();

    //Not: Bu middleware, app.UseRouting() ve app.UseAuthorization() sonrası, ama app.UseEndpoints()(bu artık kullanılmıyor) veya app.MapControllers() öncesine eklenmeli.
    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Host başlatılırken kritik hata olursa burada yakala ve Serilog'a bildir
    Log.Fatal(ex, "Host startıda fatal bir hata oluştu");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
