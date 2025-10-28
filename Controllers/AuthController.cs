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
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController<AuthRepository>
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthController(IConfiguration configuration, AuthService service,
                              AuthRepository repository, ILogger<AuthController> logger)  : base (repository, logger)
        {
            _configuration = configuration;
            _service = service;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (await _repository.GetAll().AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
                {
                    _logger.LogWarning($"{WARNING} Имя - {model.Username} или почта - {model.Email} уже существует");
                    return BadRequest(new { description = "Имя или почта уже существует" });
                }

                var hashedPassword = _service.HashPassword(model.Password);

                var user = new UserModel
                {
                    Username = model.Username,
                    PasswordHash = hashedPassword,
                    Email = model.Email,
                    Timestamp = DateTime.UtcNow
                };

                await _repository.Create(user);

                _logger.LogInformation($"{SUCCESS} Пользователь {model.Username} успешно создан");

                return Ok(new { message = "Пользователь успешно создан" });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при создании пользователя: {ex.Message}");
                return BadRequest(new { description = "Ошибка при регистрации пользователя" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _repository.GetAll().FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null || !_service.VerifyPassword(model.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"{WARNING} Неверное имя пользователя или пароль");
                    return Unauthorized(new { description = "Неверное имя пользователя или пароль" });
                }

                var token = GenerateJwtToken(user);

                _logger.LogInformation($"{SUCCESS} Успешный вход пользователя {model.Username} в учетную запись");
                return Ok(new { token });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при входе в учетную запись: {ex.Message}");
                return BadRequest(new { description = "Ошибка при входе в учетную запись" });
            }
        }

        private string GenerateJwtToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12), 
                signingCredentials: credentials
            );

            _logger.LogInformation($"{SUCCESS} JWT Токен для пользователя {user.Username} успешно создан");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}