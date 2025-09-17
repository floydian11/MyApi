using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Results;

namespace MyApi.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        // Bu metod, Result nesnesini alıp uygun HTTP cevabına dönüştürür.
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                // Eğer T'nin tipi 'bool' ise ve değeri 'false' ise, bu genellikle
                // bir 'update' veya 'delete' işleminin başarısız olduğu anlamına gelebilir.
                // Bu özel durumu ele alabiliriz, ama şimdilik basit tutalım.
                return Ok(result.Value);
            }

            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error),
                ErrorType.Validation => BadRequest(result.Error),
                ErrorType.Conflict => Conflict(result.Error),
                // Varsayılan olarak 500 Internal Server Error dönmek daha güvenli olabilir.
                _ => StatusCode(500, result.Error)
            };
        }

        // Veri döndürmeyen Result için de bir overload (aşırı yükleme) yazalım.
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent(); // Başarılı ama içerik yok -> 204 NoContent
            }

            // ... yukarıdaki switch-case'in aynısı ...
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error),
                ErrorType.Validation => BadRequest(result.Error),
                ErrorType.Conflict => Conflict(result.Error),
                _ => StatusCode(500, result.Error)
            };
        }
    }
}
