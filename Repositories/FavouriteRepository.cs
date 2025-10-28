using InvestmentSimulatorAPI.Models.Database;
using InvestmentSimulatorAPI.Interfaces;
using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Exceptions;

namespace InvestmentSimulatorAPI.Repositories
{
    public class FavouriteRepository : IBaseRepository<FavouritesModel>
    {
        private ApplicationDbContext _dbContext;
        public FavouriteRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Create(FavouritesModel entity)
        {
            try
            {
                await _dbContext.Favourites.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при создании избранного: {ex}", entity.Id);
            }
        }

        public async Task Delete(FavouritesModel entity)
        {
            try
            {
                _dbContext.Favourites.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при удалении избранного: {ex}", entity.Id);
            }
        }

        public IQueryable<FavouritesModel> GetAll()
        {
            return _dbContext.Favourites;
        }

        public IQueryable<FavouritesModel> GetAllByUserIdAsync(int userId)
        {
            return _dbContext.Favourites.Where(f => f.UserId == userId);
        }
    }
}