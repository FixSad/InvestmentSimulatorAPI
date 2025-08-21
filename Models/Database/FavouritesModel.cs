using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class FavouritesModel
    {
        [Key]
        public int Id { get; set; }
        public required string Symbol { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;
    }
}