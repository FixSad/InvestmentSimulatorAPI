using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class BaseController<IBaseRepository> : ControllerBase
{
    private protected readonly string SUCCESS = $"[SUCCESS | {DateTime.UtcNow}]";
    private protected readonly string ERROR = $"[ERROR | {DateTime.UtcNow}]";
    private protected readonly string WARNING = $"[WARNING | {DateTime.UtcNow}]";
    private protected readonly IBaseRepository _repository;
    private protected readonly ILogger<BaseController<IBaseRepository>> _logger;

    public BaseController(IBaseRepository repository, ILogger<BaseController<IBaseRepository>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    protected int GetCurrentUserId()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID не найден в токене JWT");
        }
        return userId;
    }
}