using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Pending|Completed|Failed")]
        public string Status { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}