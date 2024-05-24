using FluentValidation;
using MiApi.DTOs;

namespace MiApi.Validators
{
    public class CategoryInsertValidator : AbstractValidator<CategoryInsertDto>
    {
        public CategoryInsertValidator() 
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage(b =>"{PropertyName} is Required." );
        }
    }
}
