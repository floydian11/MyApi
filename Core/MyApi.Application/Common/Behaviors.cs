using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Common
{
    // Bu sınıf, MediatR pipeline'ına girerek her bir isteği (TRequest)
    // ilgili işleyiciye (Handler) gitmeden önce yakalar.
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        // DI ile o anki TRequest için kayıtlı olan tüm validator'ları alıyoruz.
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Eğer bu isteği doğrulayacak bir validator yoksa, devam et.
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            // Tüm validator'ları çalıştır ve hataları topla.
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // Eğer herhangi bir hata varsa...
            if (failures.Count != 0)
            {
                // Bir ValidationException fırlat. Bu, pipeline'ı durdurur.
                // Handler HİÇ ÇALIŞMAZ.
                // Hata, bizim ExceptionMiddleware'imiz tarafından yakalanır.
                throw new ValidationException(failures);
            }

            // Hata yoksa, pipeline'daki bir sonraki adıma (veya asıl Handler'a) devam et.
            return await next();
        }
    }
}
