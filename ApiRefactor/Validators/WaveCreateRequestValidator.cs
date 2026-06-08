using ApiRefactor.DTO;
using FluentValidation;

namespace ApiRefactor.Validators
{
    public class WaveCreateRequestValidator : AbstractValidator<WaveCreateRequestDto>
    {
        public WaveCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.WaveDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
