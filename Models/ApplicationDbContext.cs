using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Models.Database;

namespace InvestmentSimulatorAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FavouritesModel> Favourites { get; set; } = null!;
        public DbSet<PortfolioModel> Portfolio { get; set; } = null!;
        public DbSet<TransactionModel> Transactions { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortfolioModel>()
                .HasOne(p => p.User)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavouritesModel>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}