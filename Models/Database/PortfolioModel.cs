using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class PortfolioModel
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public float Quantity { get; set; }
    }
}