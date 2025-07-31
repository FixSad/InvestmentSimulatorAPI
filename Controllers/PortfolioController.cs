using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private PortfolioRepository _repository;
        private PortfolioService _service;

        public PortfolioController(PortfolioRepository repository, PortfolioService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio([FromBody] PortfolioModel model)
        {
            try
            {
                var portfolio = new PortfolioModel()
                {
                    Symbol = model.Symbol,
                    Quantity = model.Quantity
                };
                await _repository.Create(portfolio);

                return Ok(new { description = "Портфолио успешно создано" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при создании портфолио: {ex}" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePortfolio(string id)
        {
            try
            {
                var findedPortfolio = await _service.GetPortfolioById(id);

                if (findedPortfolio is null)
                    return BadRequest(new { description = "Ошибка при удалении портфолио" });

                await _repository.Delete(findedPortfolio);

                return Ok(new { description = "Портфолио успешно удалено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при удалении портфолио: {ex}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioModel>>> GetAllPortfolio()
        {
            try
            {
                var portfolio = await _repository.GetAll().ToListAsync();

                if (portfolio == null || !portfolio.Any())
                    return NoContent();

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при получении списка портфолио: {ex.Message}" });
            }
        }
    }
}