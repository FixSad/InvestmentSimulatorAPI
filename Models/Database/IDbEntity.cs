using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulatorAPI.Models.Database;

public interface IEntity
{
    int Id { get; set; }
}