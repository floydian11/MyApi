using FluentValidation;
using MyApi.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Validators
{
    public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Müşteri adı boş olamaz.")
                .MaximumLength(100).WithMessage("Müşteri adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Ödeme şekli boş olamaz.")
                .MaximumLength(80).WithMessage("Ödeme şekli 80 karakterden uzun olamaz.");

            RuleFor(x => x.OrderDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Sipariş tarihi bugünden ileri olamaz.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notlar 500 karakterden uzun olamaz.");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Sipariş en az bir ürün içermelidir.");

            RuleForEach(x => x.OrderItems).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId)
                     .NotEmpty().WithMessage("Ürün Id boş olamaz.");

                items.RuleFor(i => i.Quantity)
                     .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");

                items.RuleFor(i => i.Discount)
                     .InclusiveBetween(0, 100).WithMessage("İndirim 0 ile 100 arasında olmalıdır.")
                     .When(i => i.Discount.HasValue);
            });
        }
    }
    
}
