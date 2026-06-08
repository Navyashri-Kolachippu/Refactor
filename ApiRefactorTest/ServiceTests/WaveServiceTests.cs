using ApiRefactor.Domain.Entities;
using ApiRefactor.DTO;
using ApiRefactor.Repositories.Interfaces;
using ApiRefactor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace ApiRefactorTest.ServiceTests
{
    public class WaveServiceTests
    {
        private readonly Mock<IWaveRepository> _repoMock;
        private readonly Mock<ILogger<WaveService>> _loggerMock;
        private readonly WaveService _service;

        public WaveServiceTests()
        {
            _repoMock = new Mock<IWaveRepository>();
            _loggerMock = new Mock<ILogger<WaveService>>();
            _service = new WaveService(_repoMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsWaves_WhenDataExists()
        {
            var waves = new List<Wave>
        {
            new Wave { Id = Guid.NewGuid(), Name = "Wave1", WaveDate = DateTime.UtcNow },
            new Wave { Id = Guid.NewGuid(), Name = "Wave2", WaveDate = DateTime.UtcNow }
        };

            _repoMock.Setup(r => r.GetAllAsync(1, 10, default))
                     .ReturnsAsync(waves);

            var result = await _service.GetAllAsync(1, 10);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ThrowsException_WhenNoData()
        {
            _repoMock.Setup(r => r.GetAllAsync(1, 10, default))
                     .ReturnsAsync(new List<Wave>());

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetAllAsync(1, 10));
        }


        [Fact]
        public async Task GetByIdAsync_ReturnsWave_WhenFound()
        {
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(id, default))
                     .ReturnsAsync(new Wave
                     {
                         Id = id,
                         Name = "Wave1",
                         WaveDate = DateTime.UtcNow
                     });

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(id, default))
                     .ReturnsAsync((Wave?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetByIdAsync(id));
        }


        [Fact]
        public async Task CreateAsync_CreatesWave_WhenValid()
        {
            var request = new WaveCreateRequestDto
            {
                Id = Guid.NewGuid(),
                Name = "Wave1",
                WaveDate = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.GetByIdAsync(request.Id, default))
                     .ReturnsAsync((Wave?)null);

            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Wave>(), default))
                     .Returns(Task.CompletedTask);

            await _service.CreateAsync(request, default);

            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Wave>(), default), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Throws_WhenWaveAlreadyExists()
        {
            var id = Guid.NewGuid();

            var request = new WaveCreateRequestDto
            {
                Id = id,
                Name = "Wave1",
                WaveDate = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.GetByIdAsync(id, default))
                     .ReturnsAsync(new Wave { Id = id });

            await Assert.ThrowsAsync<ValidationException>(() =>
                _service.CreateAsync(request, default));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesWave_WhenExists()
        {
            var id = Guid.NewGuid();

            var existing = new Wave { Id = id, Name = "Old", WaveDate = DateTime.UtcNow };

            _repoMock.Setup(r => r.GetByIdAsync(id, default))
                     .ReturnsAsync(existing);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Wave>(), default))
                     .Returns(Task.CompletedTask);

            var request = new WaveUpdateRequestDto
            {
                Name = "Updated",
                WaveDate = DateTime.UtcNow
            };

            await _service.UpdateAsync(id, request);

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Wave>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(id, default))
                     .ReturnsAsync((Wave?)null);

            var request = new WaveUpdateRequestDto
            {
                Name = "Updated",
                WaveDate = DateTime.UtcNow
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateAsync(id, request));
        }
    }
}
