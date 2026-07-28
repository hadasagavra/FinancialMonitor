using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Tests
{
    public class TestAppDbContext : DbContext, IAppDbContext
    {
        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        public async Task Save()
        {
            await SaveChangesAsync();
        }
    }
}