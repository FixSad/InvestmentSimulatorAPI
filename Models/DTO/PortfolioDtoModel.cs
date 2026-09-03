using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.DTO
{
    public class PortfolioDtoModel 
    {
        [Required]
        public string? Symbol { get; set; }
        [Required]
        public float? Quantity { get; set; }
    }
}