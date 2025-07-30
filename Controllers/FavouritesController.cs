using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Repositories;
using InvestmentSimulatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Controllers
{
    [Route("api/favourites")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private FavouriteRepository _repository;
        private FavouriteService _service;

        public FavouritesController(FavouriteRepository repository, FavouriteService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavourite([FromBody] FavouritesModel model)
        {
            try
            {
                var favourite = new FavouritesModel()
                {
                    Symbol = model.Symbol
                };
                await _repository.Create(favourite);

                return Ok(new { description = "Избранное успешно создано" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при создании избранного: {ex}" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFavourite(string id)
        {
            try
            {
                var findedFavourite = await _service.GetFavouriteById(id);

                if (findedFavourite is null)
                    return BadRequest(new { description = "Ошибка при удалении избранного" });

                await _repository.Delete(findedFavourite);

                return Ok(new { description = "Избранное успешно удалено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при удалении избранного: {ex}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouritesModel>>> GetAllFavourites()
        {
            try
            {
                var favourites = await _repository.GetAll().ToListAsync();

                if (favourites == null || !favourites.Any())
                    return NoContent();

                return Ok(favourites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { description = $"Ошибка при получении списка избранных: {ex.Message}" });
            }
        }
    }
}