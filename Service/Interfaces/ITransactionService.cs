using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Service.Interfaces
{
    public interface ITransactionService
    {
        Task AddAsync(TransactionDto transaction);
        Task<IReadOnlyList<TransactionDto>> GetLatestAsync(int count);
    }
}
