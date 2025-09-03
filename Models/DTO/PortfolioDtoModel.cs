namespace InvestmentSimulatorAPI.Models.DTO
{
    public class PortfolioDtoModel
    {
        public int Id { get; set; }
        public required string Symbol { get; set; }
        public float Quantity { get; set; }
    }
}