using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Antiforgery;

namespace WeChooz.TechAssessment.Web.Authentication;

[ApiController]
[Route("_api/auth")]
public sealed class AuthStatusController : ControllerBase
{
    private readonly IAntiforgery _antiforgery;

    public AuthStatusController(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery;
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return NoContent();
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public ActionResult<object> Get()
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
        {
            return Ok(new { isAuthenticated = false, roles = Array.Empty<string>() });
        }

        var roles = User.Claims
            .Where(c => c.Type.EndsWith("role", StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .Distinct()
            .ToArray();

        return Ok(new { isAuthenticated = true, roles });
    }

    [HttpGet("csrf")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public ActionResult<object> GetCsrfToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }
}
