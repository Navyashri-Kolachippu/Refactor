using ApiRefactor.DTO;
using ApiRefactor.Endpoints;
using ApiRefactor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiRefactorTest.EndpointsTests
{
    public class WaveControllerTests
    {
        private readonly Mock<IWaveService> _serviceMock;
        private readonly Mock<ILogger<WaveController>> _loggerMock;
        private readonly WaveController _controller;

        public WaveControllerTests()
        {
            _serviceMock = new Mock<IWaveService>();
            _loggerMock = new Mock<ILogger<WaveController>>();

            _controller = new WaveController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithData()
        {
            var expected = new List<WaveResponseDto>
        {
            new WaveResponseDto { Id = Guid.NewGuid(), Name = "Wave1" }
        };

            _serviceMock
                .Setup(s => s.GetAllAsync(1, 10, default))
                .ReturnsAsync(expected);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<WaveResponseDto>>(okResult.Value);

            Assert.Single(value);
        }


        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var id = Guid.NewGuid();

            var dto = new WaveResponseDto
            {
                Id = id,
                Name = "Wave1"
            };

            _serviceMock
                .Setup(s => s.GetByIdAsync(id, default))
                .ReturnsAsync(dto);

            var result = await _controller.GetById(id, default);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<WaveResponseDto>(okResult.Value);

            Assert.Equal(id, value.Id);
        }


        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var request = new WaveCreateRequestDto
            {
                Id = Guid.NewGuid(),
                Name = "Wave1",
                WaveDate = DateTime.UtcNow
            };

            _serviceMock
                .Setup(s => s.CreateAsync(request, default))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(request, default);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("GetById", createdResult.ActionName);
            Assert.Equal(request.Id, createdResult.RouteValues["id"]);
        }


        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            var id = Guid.NewGuid();

            var request = new WaveUpdateRequestDto
            {
                Name = "Updated",
                WaveDate = DateTime.UtcNow
            };

            _serviceMock
                .Setup(s => s.UpdateAsync(id, request, default))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(id, request, default);

            Assert.IsType<NoContentResult>(result);
        }


        [Fact]
        public async Task Create_CallsServiceOnce()
        {
            var request = new WaveCreateRequestDto
            {
                Id = Guid.NewGuid(),
                Name = "Wave1",
                WaveDate = DateTime.UtcNow
            };

            _serviceMock
                .Setup(s => s.CreateAsync(request, default))
                .Returns(Task.CompletedTask);

            await _controller.Create(request, default);

            _serviceMock.Verify(s => s.CreateAsync(request, default), Times.Once);
        }
    }
}
