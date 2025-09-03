namespace InvestmentSimulatorAPI.Models.DTO
{
    public class TransactionDtoModel
    {
        public int Id { get; set; }
        public required string Symbol { get; set; }
        public required string Type { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}