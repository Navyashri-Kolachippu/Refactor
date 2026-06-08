using ApiRefactor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactor.Data
{
    public class WaveDbContext : DbContext
    {
        public WaveDbContext(DbContextOptions<WaveDbContext> options)
            : base(options)
        {
        }

        public DbSet<Wave> Waves => Set<Wave>();
    }
}
