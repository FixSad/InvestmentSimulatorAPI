namespace InvestmentSimulatorAPI.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);
        Task Delete(T entity);
        IQueryable<T> GetAll();
    }
}