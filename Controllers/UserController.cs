using Microsoft.AspNetCore.Mvc;
using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace InvestmentSimulatorAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly AuthRepository _repository;
        private readonly ILogger<AuthController> _logger;
        private readonly string SUCCESS = $"[SUCCESS | {DateTime.UtcNow}]";
        private readonly string ERROR = $"[ERROR | {DateTime.UtcNow}]";
        private readonly string WARNING = $"[WARNING | {DateTime.UtcNow}]";

        public UserController(AuthRepository repository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _repository.GetUserById(userId);

                if (user == null)
                {
                    return NotFound(new { message = "Пользователь не найден." });
                }

                UserDtoModel dto = new()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Timestamp = user.Timestamp,
                    IsAdmin = user.IsAdmin,
                    Transactions = user.Transactions?.Select(t => new TransactionDtoModel
                    {
                        Id = t.Id,
                        Symbol = t.Symbol,
                        Type = t.Type,
                        Quantity = t.Quantity,
                        Price = t.Price,
                        Timestamp = t.Timestamp
                    }).ToList(),
                    Portfolios = user.Portfolios?.Select(p => new PortfolioDtoModel
                    {
                        Id = p.Id,
                        Symbol = p.Symbol,
                        Quantity = p.Quantity
                    }).ToList(),
                    Favourites = user.Favourites?.Select(f => new FavouriteDtoModel
                    {
                        Id = f.Id,
                        Symbol = f.Symbol
                    }).ToList()
                };

                _logger.LogInformation($"{SUCCESS} Пользователь с id {userId} успешно получен");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении данных пользователя: {ex.Message}");
                return BadRequest(new { description = "Ошибка при получении данных пользователя" });
            }
        }
    }
}