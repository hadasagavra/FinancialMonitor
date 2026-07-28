using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly ITransactionNotifier _notifier;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionRepository repository,
            ITransactionNotifier notifier,
            IMapper mapper)
        {
            _repository = repository;
            _notifier = notifier;
            _mapper = mapper;
        }

        public async Task AddAsync(TransactionDto transaction)
        {
            var entity = _mapper.Map<Transaction>(transaction);
            await _repository.AddAsync(entity);
            var saved = _mapper.Map<TransactionDto>(entity);
            await _notifier.Broadcast(saved);
        }

        public async Task<IReadOnlyList<TransactionDto>> GetLatestAsync(int count)
        {
            var entities = await _repository.GetLatestAsync(count);
            return _mapper.Map<List<TransactionDto>>(entities);
        }
    }
}