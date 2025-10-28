using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Attributes;
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : BaseController<TransactionRepository>
    {
        private TransactionService _service;

        public TransactionController(TransactionRepository repository, TransactionService service,
                                     ILogger<TransactionController> logger) : base(repository, logger)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var transaction = new TransactionModel()
                {
                    Symbol = model.Symbol,
                    Quantity = model.Quantity,
                    Type = model.Type,
                    Price = model.Price,
                    Timestamp = DateTime.UtcNow,
                    UserId = userId
                };
                await _repository.Create(transaction);

                _logger.LogInformation(
                    $@"{SUCCESS} Транзакция с данными Symbol - {model.Symbol}, Quantity - {model.Quantity}
                    , Type - {model.Type}, Price - {model.Price} успешно создана"
                 );
                return Ok(new { description = "Транзакция успешно создана" });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $@"{SUCCESS} Ошибка при создании транзакции с данными Symbol - {model.Symbol}, 
                    Quantity - {model.Quantity}, Type - {model.Type}, Price - {model.Price}: {ex.Message}"
                 );
                return BadRequest(new { description = $"Ошибка при создании транзакции" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                TransactionModel findedTransaction = await _service.GetTransactionById(id);

                if (findedTransaction is null)
                {
                    _logger.LogWarning($"{WARNING} Транзация с ID {id} не была найдена");
                    return BadRequest(new { description = "Ошибка при удалении транзакции" });
                }

                await _repository.Delete(findedTransaction);

                _logger.LogInformation($"{SUCCESS} Транзакция с ID {id} успешно удалена");
                return Ok(new { description = "Транзакция успешно удалена" });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при удалении транзакции с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при удалении транзакции" });
            }
        }

        [HttpGet]
        [AdminOnly]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetAllTransactions()
        {
            try
            {
                List<TransactionModel> transactions = await _repository.GetAll().ToListAsync();

                if (transactions == null || !transactions.Any())
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Список транзакций успешно получен");
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении списка транзакций: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении списка транзакций" });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTransaction(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                TransactionModel transaction = await _service.GetTransactionById(id);

                if (transaction == null)
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Транзакция с ID {id} успешно получена");
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении транзакции с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении транзакций c ID {id}" });
            }
        }
    }
}