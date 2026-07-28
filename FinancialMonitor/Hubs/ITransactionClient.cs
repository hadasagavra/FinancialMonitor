using Common.Dto;

namespace FinancialMonitor.Hubs
{
    public interface ITransactionClient
    {
        Task ReceiveTransaction(TransactionDto transaction);
    }
}