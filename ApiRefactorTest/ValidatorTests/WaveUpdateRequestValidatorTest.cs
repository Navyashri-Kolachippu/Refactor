using ApiRefactor.DTO;
using ApiRefactor.Validators;
using FluentValidation.TestHelper;

namespace ApiRefactorTest.ValidatorTests
{
    public class WaveUpdateRequestValidatorTest
    {
        private readonly WaveUpdateRequestValidator _validator = new();

        [Fact]
        public void Should_Fail_ForUpdate_When_Name_Is_Empty()
        {
            var model = new WaveUpdateRequestDto
            {
                Name = "",
                WaveDate = DateTime.UtcNow
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_ForUpdate_When_Name_Too_Long()
        {
            var model = new WaveUpdateRequestDto
            {
                Name = new string('A', 101),
                WaveDate = DateTime.UtcNow
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Pass_ForUpdate_When_Valid()
        {
            var model = new WaveUpdateRequestDto
            {
                Name = "Valid Name",
                WaveDate = DateTime.UtcNow.AddDays(-1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

