using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Models.Database;

namespace InvestmentSimulatorAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FavoutitesModel> favoutites { get; set; }
        public DbSet<PortfolioModel> portfolio { get; set; }
        public DbSet<TransactionModel> transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = investment.db");
        }
    }
}