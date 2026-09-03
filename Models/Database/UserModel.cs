using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class UserModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; } = false;

        public ICollection<TransactionModel>? Transactions { get; set; }
        public ICollection<PortfolioModel>? Portfolios { get; set; }
        public ICollection<FavouritesModel>? Favourites { get; set; } 
    }
}