using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Api.Extensions;
using MyApi.Api.Middlewares;
using MyApi.Api.Seed;
using MyApi.Application.Extensions;
using MyApi.Application.Mapping;
using MyApi.Domain.Entities.Identity;
using MyApi.ExternalServices.Extensions;
using MyApi.Persistence.Context;
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

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.WebRootPath);
builder.Services.AddPersistenceServices(builder.Configuration); builder.Services.AddApiValidators();                              // Validator pipeline (controller/service için)

builder.Services.AddHttpContextAccessor();

// 2. Identity servislerini ekle
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // User
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); // Email confirmation / password reset tokenleri için

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

    //SEED DATA

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await SeedData.Initialize(services);
    }

    // isteğe bağlı: istersen HTTP request loglarını otomatik almak için
    // app.UseSerilogRequestLogging(); // çok fazla request logu istemiyorsan kapalı bırak

    app.UseHttpsRedirection();
    app.UseStaticFiles();// default: wwwroot klasörünü açar
    app.UseRouting();

    app.UseCors("AllowAll"); // eğer policy tanımlandıysa
    app.UseAuthentication();
    app.UseAuthorization();

   

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
    Log.Fatal(ex, "Host başlatılmasında fatal bir hata oluştu");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
