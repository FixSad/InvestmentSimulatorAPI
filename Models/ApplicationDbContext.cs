using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Models.Database;

namespace InvestmentSimulatorAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FavoutitesModel> Favourites { get; set; }
        public DbSet<PortfolioModel> Portfolio { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = investment.db");
        }
    }
}