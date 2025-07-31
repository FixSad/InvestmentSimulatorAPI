using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Services
{
    public class PortfolioService
    {
        private ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context) => _context = context;

        public async Task<PortfolioModel> GetPortfolioById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));
                }

                var findedPortfolio = await _context.Portfolio.SingleOrDefaultAsync(f => f.Id.ToString() == id);

                if (findedPortfolio is null)
                {
                    return null;
                }

                return findedPortfolio;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[ERR] Ошибка при получении портфолио с ID: {id}: {ex.Message}", ex);
            }
        }
    }
}