using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.DTO
{
    public class FundDtoModel
    {
        [Required]
        public int? Funds { get; set; }
        [Required]
        public string? Symbol { get; set; }
    } 
}