namespace InvestmentSimulatorAPI.Models.Database;

public interface IUserProduct
{
    int Id { get; set; }
    int UserId { get; set; }
    string Symbol { get; set; }
}