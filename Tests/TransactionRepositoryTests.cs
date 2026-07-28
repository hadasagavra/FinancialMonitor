using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
namespace Tests
{
    public class TransactionRepositoryTests
    {
        private static TestAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestAppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_PersistsTransaction()
        {
            await using var context = CreateContext();
            var repository = new TransactionRepository(context);
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 1500.50m,
                Currency = "USD",
                Status = TransactionStatus.Pending,
                Timestamp = DateTime.UtcNow
            };

            await repository.AddAsync(transaction);

            var stored = await context.Transactions.SingleAsync();
            Assert.Equal(transaction.TransactionId, stored.TransactionId);
            Assert.Equal(1500.50m, stored.Amount);
        }

        [Fact]
        public async Task GetLatestAsync_ReturnsNewestFirst_LimitedByCount()
        {
            await using var context = CreateContext();
            var repository = new TransactionRepository(context);
            var older = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 10m,
                Currency = "USD",
                Status = TransactionStatus.Completed,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            };
            var newer = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 20m,
                Currency = "USD",
                Status = TransactionStatus.Completed,
                Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
            };
            await repository.AddAsync(older);
            await repository.AddAsync(newer);

            var latest = await repository.GetLatestAsync(1);

            Assert.Single(latest);
            Assert.Equal(newer.TransactionId, latest[0].TransactionId);
        }
    }
}