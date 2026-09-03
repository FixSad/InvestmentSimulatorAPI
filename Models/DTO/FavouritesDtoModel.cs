using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.DTO
{
    public class FavouriteDtoModel 
    {
        [Required]
        public required string? Symbol { get; set; }
    }
}