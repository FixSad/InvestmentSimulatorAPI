using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Services;
using InvestmentSimulatorAPI.Repositories;
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

                // Возвращаем данные пользователя
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении данных пользователя пользователя: {ex.Message}");
                return BadRequest(new { description = "Ошибка при получении данных пользователя пользователя" });
            }
        }
    }
}