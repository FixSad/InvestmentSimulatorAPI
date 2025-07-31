using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private TransactionRepository _repository;
        private TransactionService _service;

        public TransactionController(TransactionRepository repository, TransactionService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel model)
        {
            try
            {
                var transaction = new TransactionModel()
                {
                    Symbol = model.Symbol,
                    Quantity = model.Quantity,
                    Type = model.Type,
                    Price = model.Price,
                    Timestamp = DateTime.UtcNow
                };
                await _repository.Create(transaction);

                return Ok(new { description = "Транзакция успешно создана" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при создании транзакции: {ex}" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            try
            {
                var findedTransaction = await _service.GetTransactionById(id);

                if (findedTransaction is null)
                    return BadRequest(new { description = "Ошибка при удалении транзакции" });

                await _repository.Delete(findedTransaction);

                return Ok(new { description = "Транзакция успешно удалена" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при удалении транзакции: {ex}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetAllTransactions()
        {
            try
            {
                var transactions = await _repository.GetAll().ToListAsync();

                if (transactions == null || !transactions.Any())
                    return NoContent();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при получении списка транзакций: {ex.Message}" });
            }
        }
    }
}