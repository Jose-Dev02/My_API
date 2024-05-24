using FluentValidation;
using MiApi.DtoS;
using MiApi.Models;

namespace MiApi.Validators
{
    public class ProductInsertValidator : AbstractValidator<ProductInsertDto>
    {
        private InventoryContext _inventoryContext;
        public ProductInsertValidator(InventoryContext inventoryContext) 
        {
            _inventoryContext = inventoryContext;
            var maxValue = _inventoryContext.Categories.ToList().Count;

            RuleFor(b => b.Name).NotEmpty().WithMessage(b => "{PropertyName} is required");
            RuleFor(b => b.Name).Length(2, 50).WithMessage(b => "{PropertyName} should be to 2 up 50 characters");
            RuleFor(b => b.Price).NotEmpty().WithMessage(b => "{PropertyName} is required");
            RuleFor(b => b.Price).GreaterThan(0).WithMessage(b => "Error with {PropertyName} value");
            RuleFor(b => b.CategoryId).NotEmpty().WithMessage(b => "{PropertyName} is required");
            RuleFor(b => b.CategoryId).GreaterThan(0).LessThanOrEqualTo(maxValue).WithMessage(b => "{PropertyName} should be between 0 -" + maxValue);
        }
    }
}
