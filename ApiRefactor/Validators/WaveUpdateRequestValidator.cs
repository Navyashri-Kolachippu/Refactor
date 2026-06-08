using ApiRefactor.DTO;
using FluentValidation;

namespace ApiRefactor.Validators
{
    public class WaveUpdateRequestValidator : AbstractValidator<WaveUpdateRequestDto>
    {
        public WaveUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.WaveDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}