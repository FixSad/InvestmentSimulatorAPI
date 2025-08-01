using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Services
{
    public class FavouriteService
    {
        private readonly ApplicationDbContext _context;

        public FavouriteService(ApplicationDbContext context) => _context = context;

        public async Task<FavouritesModel> GetFavouriteById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("Идентификатор не может быть пустым", nameof(id));
                }

                var findedFavourite = await _context.Favourites.SingleOrDefaultAsync(f => f.Id.ToString() == id);

                if (findedFavourite is null)
                {
                    throw new KeyNotFoundException($"Избранное с ID {id} не найдено");
                } 

                return findedFavourite;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[ERR] Ошибка при получении избранного с ID: {id}: {ex.Message}", ex);
            }
        }
    }
}