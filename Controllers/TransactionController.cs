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
        private readonly ILogger<TransactionController> _logger;
        private readonly string SUCCESS = $"[SUCCESS | {DateTime.UtcNow}]";
        private readonly string ERROR = $"[ERROR | {DateTime.UtcNow}]";
        private readonly string WARNING = $"[WARNING | {DateTime.UtcNow}]";

        public TransactionController(TransactionRepository repository, TransactionService service,
                                     ILogger<TransactionController> logger)
        {
            _repository = repository;
            _service = service;
            _logger = logger;
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

                _logger.LogInformation(
                    $@"{SUCCESS} Транзакция с данными Symbol - {model.Symbol}, Quantity - {model.Quantity}
                    , Type - {model.Type}, Price - {model.Price} успешно создана"
                 );
                return Ok(new { description = "Транзакция успешно создана" });
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
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при удалении транзакции с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при удалении транзакции" });
            }
        }

        [HttpGet]
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