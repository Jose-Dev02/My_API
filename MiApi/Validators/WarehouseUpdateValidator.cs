using FluentValidation;
using MiApi.DTOs;
using MiApi.Models;

namespace MiApi.Validators
{
    public class WarehouseUpdateValidator : AbstractValidator<WarehouseUpdateDto>
    {
        public WarehouseUpdateValidator() 
        {
            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage(b => "{PropertyName} cannot be NULL or being Empty.");
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage(b => "{PropertyName} should be between 0 - ");
            RuleFor(b => b.Cantidad).NotEmpty().NotNull().WithMessage(b => "{PropertyName} cannot be NULL or being Empty.");
            RuleFor(b => b.Cantidad).GreaterThan(-1).WithMessage(b => "{PropertyName} cannot take negative values.");
        }
    }
}
