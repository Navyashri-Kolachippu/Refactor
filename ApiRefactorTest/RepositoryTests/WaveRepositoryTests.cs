using ApiRefactor.Data;
using ApiRefactor.Domain.Entities;
using ApiRefactor.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactorTest.RepositoryTests
{
    public class WaveRepositoryTests
    {
        private WaveDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<WaveDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new WaveDbContext(options);
        }

        private WaveRepository CreateRepository(WaveDbContext context)
        {
            return new WaveRepository(context);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsPagedResults()
        {
            var context = CreateDbContext();

            context.Waves.AddRange(
                new Wave { Id = Guid.NewGuid(), Name = "A", WaveDate = DateTime.UtcNow },
                new Wave { Id = Guid.NewGuid(), Name = "B", WaveDate = DateTime.UtcNow },
                new Wave { Id = Guid.NewGuid(), Name = "C", WaveDate = DateTime.UtcNow }
            );

            await context.SaveChangesAsync();

            var repo = CreateRepository(context);

            var result = await repo.GetAllAsync(1, 2, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task GetByIdAsync_ReturnsWave_WhenExists()
        {
            var context = CreateDbContext();

            var id = Guid.NewGuid();

            context.Waves.Add(new Wave
            {
                Id = id,
                Name = "Test",
                WaveDate = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var repo = CreateRepository(context);

            var result = await repo.GetByIdAsync(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var context = CreateDbContext();
            var repo = CreateRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
        }


        [Fact]
        public async Task InsertAsync_SavesWaveToDatabase()
        {
            var context = CreateDbContext();
            var repo = CreateRepository(context);

            var wave = new Wave
            {
                Id = Guid.NewGuid(),
                Name = "NewWave",
                WaveDate = DateTime.UtcNow
            };

            await repo.InsertAsync(wave, CancellationToken.None);

            var saved = await context.Waves.FirstOrDefaultAsync(x => x.Id == wave.Id);

            Assert.NotNull(saved);
            Assert.Equal("NewWave", saved!.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingWave()
        {
            var context = CreateDbContext();

            var id = Guid.NewGuid();

            context.Waves.Add(new Wave
            {
                Id = id,
                Name = "OldName",
                WaveDate = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var repo = CreateRepository(context);

            var updatedWave = new Wave
            {
                Id = id,
                Name = "UpdatedName",
                WaveDate = DateTime.UtcNow
            };

            await repo.UpdateAsync(updatedWave, CancellationToken.None);

            var result = await context.Waves.FirstOrDefaultAsync(x => x.Id == id);

            Assert.Equal("UpdatedName", result!.Name);
        }
    }
}
