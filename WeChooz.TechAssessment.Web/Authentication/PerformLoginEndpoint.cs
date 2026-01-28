using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WeChooz.TechAssessment.Web.Authentication;

[Route("_api/admin/login")]
public class PerformLoginEndpoint : Ardalis.ApiEndpoints.EndpointBaseAsync.WithRequest<PerformLoginRequest>
    .WithActionResult
{
    [HttpPost]
    public override async Task<ActionResult> HandleAsync(PerformLoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return BadRequest("Login cannot be empty.");
        }
        if (request.Login == "formation" || request.Login == "sales")
        {
            var identity = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Role, request.Login), new Claim(ClaimTypes.Name, request.Login) },
                "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Cookies", principal);
            return Ok(new { login = request.Login });
        }
        return Unauthorized();
    }
}
