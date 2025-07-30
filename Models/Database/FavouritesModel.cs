using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class FavouritesModel
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
}