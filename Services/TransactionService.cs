using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Services
{
    public class TransactionService
    {
        private ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context) => _context = context;

        public async Task<TransactionModel> GetTransactionById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));
                }

                var findedTransaction = await _context.Transactions.SingleOrDefaultAsync(f => f.Id.ToString() == id);

                if (findedTransaction is null)
                {
                    return null;
                }

                return findedTransaction;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[ERR] Ошибка при получении транзакции с ID: {id}: {ex.Message}", ex);
            }
        }
    }
}