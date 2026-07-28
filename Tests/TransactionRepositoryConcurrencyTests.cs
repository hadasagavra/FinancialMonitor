using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Repositories;

namespace Tests
{
    public class TransactionRepositoryConcurrencyTests
    {
        [Fact]
        public async Task AddAsync_HandlesConcurrentWrites_WithoutLoss()
        {
            const int concurrentWrites = 100;
            var connectionString = $"Data Source=file:{Guid.NewGuid()}?mode=memory&cache=shared";

            await using var keepAlive = new SqliteConnection(connectionString);
            await keepAlive.OpenAsync();
            await EnsureSchema(connectionString);

            var writes = Enumerable.Range(0, concurrentWrites)
                .Select(_ => WriteOne(connectionString));
            await Task.WhenAll(writes);

            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            await using var verify = CreateContext(connection);
            Assert.Equal(concurrentWrites, await verify.Transactions.CountAsync());
        }

        private static async Task WriteOne(string connectionString)
        {
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            await SetBusyTimeout(connection);

            await using var context = CreateContext(connection);
            var repository = new TransactionRepository(context);
            await repository.AddAsync(NewTransaction());
        }

        private static async Task EnsureSchema(string connectionString)
        {
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            await using var context = CreateContext(connection);
            await context.Database.EnsureCreatedAsync();
        }

        private static TestAppDbContext CreateContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<TestAppDbContext>()
                .UseSqlite(connection)
                .Options;

            return new TestAppDbContext(options);
        }

        private static async Task SetBusyTimeout(SqliteConnection connection)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA busy_timeout = 5000;";
            await command.ExecuteNonQueryAsync();
        }

        private static Transaction NewTransaction()
        {
            return new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 100m,
                Currency = "USD",
                Status = TransactionStatus.Completed,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}