using ApiRefactor.DTO;

namespace ApiRefactor.Services.Interfaces
{
    public interface IWaveService
    {
       Task<IEnumerable<WaveResponseDto>> GetAllAsync(
       int pageNumber,
       int pageSize,
       CancellationToken cancellationToken = default);

        Task<WaveResponseDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            WaveCreateRequestDto request,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Guid id,
            WaveUpdateRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
