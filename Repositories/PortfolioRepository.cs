using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Interfaces;
using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Repositories
{
    public class PortfolioRepository : IBaseRepository<PortfolioModel>
    {
        private ApplicationDbContext _dbContext;

        public PortfolioRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Create(PortfolioModel entity)
        {
            try
            {
                await _dbContext.Portfolio.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при создании портфолио: {ex}", entity.Id);
            }
        }

        public async Task Delete(PortfolioModel entity)
        {
            try
            {
                _dbContext.Portfolio.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при удалении портфолио: {ex}", entity.Id);
            }
        }

        public IQueryable<PortfolioModel> GetAll()
        {
            return _dbContext.Portfolio;
        }
    }
}