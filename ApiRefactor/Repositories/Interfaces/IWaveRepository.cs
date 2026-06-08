using ApiRefactor.Domain.Entities;

namespace ApiRefactor.Repositories.Interfaces
{
    public interface IWaveRepository
    {
        Task<IReadOnlyList<Wave>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

        Task<Wave?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task InsertAsync(
            Wave wave,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Wave wave,
            CancellationToken cancellationToken);
    }
}
