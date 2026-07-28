using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IAppDbContext _context;

        public TransactionRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.Save();
        }

        public async Task<IReadOnlyList<Transaction>> GetLatestAsync(int count)
        {
            return await _context.Transactions
                .OrderByDescending(t => t.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}