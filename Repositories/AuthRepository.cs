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
                throw new ApplicationException($"[ERR] Ошибка при создании пользователя: {ex}");
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
                throw new ApplicationException($"[ERR] Ошибка при удалении пользователя: {ex}");
            }
        }

        public IQueryable<UserModel> GetAll()
        {
            return _context.Users;
        }
        
        public async Task<UserModel?> GetUserById(int userId)
        {
            return await _context.Users.Where(f => f.Id == userId).FirstOrDefaultAsync();
        }
    }
}