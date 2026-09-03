using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models
{
    public class RegisterDtoModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}