using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Interfaces;
using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Repositories
{
    public class TransactionRepository : IBaseRepository<TransactionModel>
    {
        private ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Create(TransactionModel entity)
        {
            try
            {
                await _dbContext.Transactions.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при создании транзакции: {ex}", entity.Id);
            }
        }

        public async Task Delete(TransactionModel entity)
        {
            try
            {
                _dbContext.Transactions.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при удалении транзакции: {ex}", entity.Id);
            }
        }

        public IQueryable<TransactionModel> GetAll()
        {
            return _dbContext.Transactions;
        }
    }
}