using FluentValidation;
using MiApi.DTOs;

namespace MiApi.Validators
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateValidator() 
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage(b => "{PropertyName} is Required");
        }
    }
}
