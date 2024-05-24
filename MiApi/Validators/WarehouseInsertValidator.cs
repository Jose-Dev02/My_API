using FluentValidation;
using MiApi.DtoS;
using MiApi.Models;

namespace MiApi.Validators
{
    public class WarehouseInsertValidator : AbstractValidator<WarehouseInsertDto>
    {
        private InventoryContext _inventoryContext;
        public WarehouseInsertValidator(InventoryContext inventoryContext) 
        {
            _inventoryContext = inventoryContext;
            var maxValue = inventoryContext.Warehouse.ToList().Count;

            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage(b => "{PropertyName} cannot be NULL or being Empty.");
            RuleFor(b => b.ProductId).GreaterThan(0).LessThanOrEqualTo(maxValue).WithMessage(b => "{PropertyName} should be between 0 - " + maxValue);
            RuleFor(b => b.Cantidad).NotEmpty().NotNull().WithMessage(b => "{PropertyName} cannot be NULL or being Empty.");
            RuleFor(b => b.Cantidad).GreaterThan(-1).WithMessage(b => "{PropertyName} cannot take negative values.");
        }
    }
}
