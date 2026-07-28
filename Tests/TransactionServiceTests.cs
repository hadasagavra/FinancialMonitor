using AutoMapper;
using Common.Dto;
using Moq;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;

namespace Tests
{
    public class TransactionServiceTests
    {
        private static TransactionService CreateService(
            Mock<ITransactionRepository> repository,
            Mock<ITransactionNotifier> notifier)
        {
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Transaction>(It.IsAny<TransactionDto>())).Returns(new Transaction());
            mapper.Setup(m => m.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(new TransactionDto());
            return new TransactionService(repository.Object, notifier.Object, mapper.Object);
        }

        [Fact]
        public async Task AddAsync_SavesThenBroadcasts()
        {
            var calls = new List<string>();
            var repository = new Mock<ITransactionRepository>();
            var notifier = new Mock<ITransactionNotifier>();

            repository.Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .Callback(() => calls.Add("save"))
                .Returns(Task.CompletedTask);
            notifier.Setup(n => n.Broadcast(It.IsAny<TransactionDto>()))
                .Callback(() => calls.Add("broadcast"))
                .Returns(Task.CompletedTask);

            var service = CreateService(repository, notifier);

            await service.AddAsync(new TransactionDto());

            Assert.Equal(new[] { "save", "broadcast" }, calls);
        }

        [Fact]
        public async Task AddAsync_WhenSaveFails_DoesNotBroadcast()
        {
            var repository = new Mock<ITransactionRepository>();
            var notifier = new Mock<ITransactionNotifier>();

            repository.Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new InvalidOperationException("db down"));

            var service = CreateService(repository, notifier);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(new TransactionDto()));
            notifier.Verify(n => n.Broadcast(It.IsAny<TransactionDto>()), Times.Never);
        }
    }
}