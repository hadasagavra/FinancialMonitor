using Common.Dto;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;

namespace FinancialMonitor.Hubs
{
    public class TransactionNotifier : ITransactionNotifier
    {
        private readonly IHubContext<TransactionHub, ITransactionClient> _hubContext;

        public TransactionNotifier(IHubContext<TransactionHub, ITransactionClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Broadcast(TransactionDto transaction)
        {
            await _hubContext.Clients.All.ReceiveTransaction(transaction);
        }
    }
}