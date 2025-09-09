using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvestmentSimulatorAPI.Attributes;
using InvestmentSimulatorAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : BaseController
    {
        private PortfolioRepository _repository;
        private PortfolioService _service;
        private readonly ILogger<PortfolioController> _logger;
        private readonly string SUCCESS = $"[SUCCESS | {DateTime.UtcNow}]";
        private readonly string ERROR = $"[ERROR | {DateTime.UtcNow}]";
        private readonly string WARNING = $"[WARNING | {DateTime.UtcNow}]";

        public PortfolioController(PortfolioRepository repository, PortfolioService service,
                                   ILogger<PortfolioController> logger)
        {
            _repository = repository;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio([FromBody] PortfolioModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var portfolio = new PortfolioModel()
                {
                    Symbol = model.Symbol,
                    Quantity = model.Quantity,
                    UserId = userId
                };
                await _repository.Create(portfolio);

                _logger.LogInformation(
                    $@"{SUCCESS} Портфолио с данными Symbol -
                     {model.Symbol}, Quantity - {model.Quantity} успешно создано"
                );                
            return Ok(new { description = "Портфолио успешно создано" });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $@"{ERROR} Ошибка при создании портфолио с данными Symbol -
                     {model.Symbol}, Quantity - {model.Quantity}: {ex.Message}"
                );    
                return BadRequest(new { description = $"Ошибка при создании портфолио" });
            }
        }

        [HttpPost]
        [Route("addFunds")]
        public async Task<IActionResult> AddFundsPortfolio([FromBody] FundDtoModel funds)
        {
            try
            {
                var userId = GetCurrentUserId();

                var portfolio = await _repository.GetAll().Where(a => a.Symbol == "BTCUSDT" && a.UserId == userId).FirstOrDefaultAsync();

                if (!float.TryParse(funds.Funds, out float quantity))
                {
                    _logger.LogError($"{ERROR} Не удалось конвертировать значение funds {funds.Funds} в число");
                    throw new InvalidOperationException("Не удалось конвертировать значение funds в число");
                }

                if (portfolio == null)
                {
                    await _repository.Create(new PortfolioModel
                    {
                        Symbol = "BTCUSDT",
                        Quantity = quantity,
                        UserId = userId
                    });

                    _logger.LogInformation(
                        $@"{SUCCESS} Баланс успешно пополнен на {funds.Funds} USDT");

                    return Ok(new { description = "Баланс успешно пополнен!" });
                }

                await _repository.AddFunds(portfolio, quantity);

                _logger.LogInformation(
                    $@"{SUCCESS} Баланс успешно пополнен на {funds.Funds} USDT");

                return Ok(new { description = "Баланс успешно пополнен!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $@"{ERROR} Ошибка при пополнении баланса на {funds.Funds}: {ex}");
                return BadRequest(new { description = $"Ошибка при пополнении баланса" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePortfolio(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var findedPortfolio = await _service.GetPortfolioById(id);

                if (findedPortfolio is null)
                {
                    _logger.LogWarning($"{WARNING} Портфолио с ID {id} не было найдено");
                    return BadRequest(new { description = "Ошибка при удалении портфолио" });
                }

                await _repository.Delete(findedPortfolio);

                _logger.LogInformation($"{SUCCESS} Портфолио с ID {id} успешно удалено");
                return Ok(new { description = "Портфолио успешно удалено" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при удалении портфолио с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при удалении портфолио: {ex.Message}" });
            }
        }

        [HttpGet]
        [AdminOnly]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<PortfolioModel>>> GetAllPortfolio()
        {
            try
            {
                var portfolio = await _repository.GetAll().ToListAsync();

                if (portfolio == null || !portfolio.Any())
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Список портфолио успешно получен");
                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении списка портфолио: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении списка портфолио: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPortfolio(string id)
        {
            try
            {
                var portfolio = await _service.GetPortfolioById(id);

                if (portfolio == null)
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Портфолио с ID {id} успешно получен");
                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении портфолио с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении портфолио с ID {id}: {ex.Message}" });
            }
        }
    }
}