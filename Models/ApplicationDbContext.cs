using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Models.Database;

namespace InvestmentSimulatorAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FavouritesModel> Favourites { get; set; }
        public DbSet<PortfolioModel> Portfolio { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}