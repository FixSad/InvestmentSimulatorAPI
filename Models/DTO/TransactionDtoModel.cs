using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.DTO
{
    public class TransactionDtoModel 
    {
        [Required]
        public required string? Symbol { get; set; }
        [Required]
        public required string? Type { get; set; }
        [Required]
        public float? Quantity { get; set; }
        [Required]
        public float? Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}