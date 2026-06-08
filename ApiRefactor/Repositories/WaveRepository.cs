using ApiRefactor.Data;
using ApiRefactor.Domain.Entities;
using ApiRefactor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactor.Repositories
{
    public class WaveRepository : IWaveRepository
    {
        private readonly WaveDbContext _dbContext;

        public WaveRepository(WaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Wave>> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Waves
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Wave?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Waves
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task InsertAsync(
            Wave wave,
            CancellationToken cancellationToken)
        {
            await _dbContext.Waves.AddAsync(
                wave,
                cancellationToken);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        public async Task UpdateAsync(
            Wave wave,
            CancellationToken cancellationToken)
        {
            _dbContext.Waves.Update(wave);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }
}
