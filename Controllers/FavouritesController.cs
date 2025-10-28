using System.Security.Claims;
using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InvestmentSimulatorAPI.Attributes;
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteController : BaseController<FavouriteRepository>
    {
        private FavouriteService _service;

        public FavouriteController(FavouriteRepository repository, FavouriteService service,
                                   ILogger<FavouriteController> logger) : base (repository, logger)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavourite([FromBody] FavouritesModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var favourite = new FavouritesModel()
                {
                    Symbol = model.Symbol,
                    UserId = userId
                };
                await _repository.Create(favourite);

                _logger.LogInformation($"{SUCCESS} Избранное {model.Symbol} успешно создано! ");
                return Ok(new { description = "Избранное успешно создано" });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при создании избранного {model.Symbol}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при создании избранного" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFavourite(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var findedFavourite = await _service.GetFavouriteById(id);

                if (findedFavourite is null)
                {
                    _logger.LogWarning($"{WARNING} Избранное с ID {id} не было найдено");
                    return BadRequest(new { description = $"Избранное с ID {id} не было найдено" });
                }

                await _repository.Delete(findedFavourite);

                _logger.LogInformation($"{SUCCESS} Избранное с ID {id} успешно удалено");
                return Ok(new { description = "Избранное успешно удалено" });
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при удалении избранного с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при удалении избранного" });
            }
        }

        [HttpGet]
        [Route("user/all")]
        public async Task<ActionResult<IEnumerable<FavouritesModel>>> GetAllFavourites()
        {
            try
            {
                var userId = GetCurrentUserId();

                var favourites = await _repository.GetAllByUserIdAsync(userId).ToListAsync();

                if (favourites == null || !favourites.Any())
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Список избранного пользователя {userId} успешно получен");
                return Ok(favourites);
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении списка избранного: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении списка избранных" });
            }
        }

        [HttpGet]
        [AdminOnly]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<FavouritesModel>>> GetAllUserFavourites()
        {
            try
            {
                var favourites = await _repository.GetAll().ToListAsync();

                if (favourites == null || !favourites.Any())
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Список избранного успешно получен");
                return Ok(favourites);
            }
            catch (DataModelException ex)
            {
                _logger.LogError($"{ERROR} {ex.Message} Id {ex.Id}");
                return BadRequest(new { description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении списка избранного: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении списка избранных" });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetFavourite(string id)
        {
            try
            {
                var userId = GetCurrentUserId();

                var favourite = await _service.GetFavouriteById(id);

                if (favourite == null)
                    return NoContent();

                _logger.LogInformation($"{SUCCESS} Избранное с ID {id} успешно получен");
                return Ok(favourite);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ERROR} Ошибка при получении избранного с ID {id}: {ex.Message}");
                return BadRequest(new { description = $"Ошибка при получении избранного с ID {id}" });
            }
        }
    }
}