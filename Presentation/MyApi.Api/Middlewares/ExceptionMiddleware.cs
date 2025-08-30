using MyApi.Application.Exceptions;
using System.Net;

namespace MyApi.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // pipeline'ın geri kalanına geç
            }
            catch (Exception ex)
            {
                // Hata logla
                _logger.LogError(ex, ex.Message);

                // HTTP status code belirle
                int statusCode = ex switch
                {
                    BadRequestException => (int)HttpStatusCode.BadRequest,        // 400
                    ForbiddenException => (int)HttpStatusCode.Forbidden,          // 403
                    UnauthorizedException => (int)HttpStatusCode.Unauthorized,   // 401
                    NotFoundException => (int)HttpStatusCode.NotFound,            // 404
                    //InternalServerErrorException => (int)HttpStatusCode.InternalServerError, // 500 manuel buna sınfı eklemedik. 
                    _ => (int)HttpStatusCode.InternalServerError                 // beklenmeyen runtime hatalar
                };

                // Kullanıcıya dönecek mesaj
                string message = ex switch
                {
                    BadRequestException => ex.Message,
                    ForbiddenException => ex.Message,
                    UnauthorizedException => ex.Message,
                    NotFoundException => ex.Message,
                    //InternalServerErrorException => ex.Message, buna sınfı eklemedik. 
                    //_ => "Beklenmeyen bir hata oluştu", // runtime hatalar için güvenli mesaj
                    _ => ex.Message
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = statusCode,
                    Message = message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
