using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Interfaces;
using InvestmentSimulatorAPI.Models.Database;

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
                throw new ApplicationException($"[ERR] Ошибка при создании транзакции: {ex}");
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
                throw new ApplicationException($"[ERR] Ошибка при удалении транзакции: {ex}");
            }
        }

        public IQueryable<TransactionModel> GetAll()
        {
            return _dbContext.Transactions;
        }
    }
}