using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class FavoutitesModel
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
}