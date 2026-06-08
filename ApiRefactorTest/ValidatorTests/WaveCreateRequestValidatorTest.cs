using ApiRefactor.DTO;
using ApiRefactor.Validators;
using FluentValidation.TestHelper;

namespace ApiRefactorTest.ValidatorTests
{
    public class WaveCreateRequestValidatorTest
    {
        private readonly WaveCreateRequestValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Name_Is_Empty()
        {
            var model = new WaveCreateRequestDto
            {
                Name = "",
                WaveDate = DateTime.UtcNow
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_When_Name_Too_Long()
        {
            var model = new WaveCreateRequestDto
            {
                Name = new string('A', 101),
                WaveDate = DateTime.UtcNow
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var model = new WaveCreateRequestDto
            {
                Id = Guid.NewGuid(),
                Name = "Valid Name",
                WaveDate = DateTime.UtcNow.AddDays(-1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
