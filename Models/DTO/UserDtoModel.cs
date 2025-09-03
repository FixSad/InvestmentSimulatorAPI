using InvestmentSimulatorAPI.Models.Database;

namespace InvestmentSimulatorAPI.Models.DTO
{
    public class UserDtoModel
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<TransactionDtoModel>? Transactions { get; set; }
        public ICollection<PortfolioDtoModel>? Portfolios { get; set; }
        public ICollection<FavouriteDtoModel>? Favourites { get; set; } 
    }
}