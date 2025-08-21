using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class BaseController : ControllerBase
{
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