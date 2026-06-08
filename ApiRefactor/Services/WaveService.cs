using ApiRefactor.Domain.Entities;
using ApiRefactor.DTO;
using ApiRefactor.Repositories.Interfaces;
using ApiRefactor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.Services
{
    public class WaveService : IWaveService
    {
        private readonly IWaveRepository _repository;
        private readonly ILogger<WaveService> _logger;

        public WaveService(IWaveRepository repository, ILogger<WaveService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<WaveResponseDto>> GetAllAsync(int pageNumber,
                                                                    int pageSize,
                                                                    CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all waves");

            var waves = await _repository.GetAllAsync(pageNumber, pageSize, cancellationToken);
            var waveList = waves.ToList();

            if (!waveList.Any())
            {
                _logger.LogWarning("No waves found in database");
                throw new KeyNotFoundException("No waves were found in database.");
            }

            _logger.LogInformation("Fetched {Count} waves", waves.Count());

            return waveList.Select(w => new WaveResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                WaveDate = w.WaveDate
            });

        }

        public async Task<WaveResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching wave with id {WaveId}", id);

            var wave = await _repository.GetByIdAsync(id, cancellationToken);

            if (wave is null)
            {
                _logger.LogWarning("Wave with id {WaveId} was not found", id);

                throw new KeyNotFoundException(
                    $"Wave with id {id} was not found");
            }

            _logger.LogInformation("Wave with id {WaveId} retrieved successfully", id);

            WaveResponseDto responseDto = new WaveResponseDto
            {
                Id = wave.Id,
                Name = wave.Name,
                WaveDate = wave.WaveDate
            };

            return responseDto;
        }

        public async Task CreateAsync(WaveCreateRequestDto request, CancellationToken cancellationToken)
        {
            var existingWave = await _repository.GetByIdAsync(request.Id,cancellationToken);

            if (existingWave is not null)
            {
                _logger.LogWarning("Wave already exists {WaveId}", request.Id);

                throw new ValidationException(
                    $"Wave with id {request.Id} already exists.");
            }

            Wave wave = new Wave
            {
                Id = request.Id,
                Name = request.Name,
                WaveDate = request.WaveDate
            };

            await _repository.InsertAsync(wave, cancellationToken);

            _logger.LogInformation("Wave created successfully {WaveId}", request.Id);

        }

        public async Task UpdateAsync(Guid id, WaveUpdateRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating wave {WaveId}", id);

            var existingWave = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingWave is null)
            {
                _logger.LogWarning("Wave not found {WaveId}", id);

                throw new KeyNotFoundException(
                    $"Wave with id {id} was not found.");
            }

            Wave wave = new Wave
            {
                Id = id,
                Name = request.Name,
                WaveDate = request.WaveDate
            };

            await _repository.UpdateAsync(wave, cancellationToken);

            _logger.LogInformation("Wave updated successfully {WaveId}", id);
        }
    }
}

