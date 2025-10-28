using InvestmentSimulatorAPI.Exceptions;
using InvestmentSimulatorAPI.Interfaces;
using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulatorAPI.Repositories
{
    public class AuthRepository : IBaseRepository<UserModel>
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context) => _context = context;

        public async Task Create(UserModel entity)
        {
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при создании пользователя: {ex}", entity.Id);
            }
        }

        public async Task Delete(UserModel entity)
        {
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataModelException($"Ошибка при удалении пользователя: {ex}", entity.Id);
            }
        }

        public IQueryable<UserModel> GetAll()
        {
            return _context.Users;
        }
        
        public async Task<UserModel?> GetUserById(int userId)
        {
            return await _context.Users
                .Include(u => u.Transactions) 
                .Include(u => u.Portfolios)
                .Include(u => u.Favourites)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}