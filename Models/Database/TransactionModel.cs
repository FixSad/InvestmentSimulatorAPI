using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}