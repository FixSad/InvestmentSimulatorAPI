namespace InvestmentSimulatorAPI.Exceptions;

internal class DataModelException : Exception
{
    public int Id { get; init; }
    public DataModelException(string message, int id) : base(message) => Id = id; 
}