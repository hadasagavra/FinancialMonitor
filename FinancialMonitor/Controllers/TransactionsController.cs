using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace FinancialMonitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TransactionDto transaction)
        {
            await _service.AddAsync(transaction);
            return Ok(transaction);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int count = 50)
        {
            var transactions = await _service.GetLatestAsync(count);
            return Ok(transactions);
        }
    }
}