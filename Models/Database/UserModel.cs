using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        public required string Username { get; set; }

        public required string PasswordHash { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; } = false;

        public ICollection<TransactionModel>? Transactions { get; set; }
        public ICollection<PortfolioModel>? Portfolios { get; set; }
        public ICollection<FavouritesModel>? Favourites { get; set; } 
    }
}