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
            var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim(ClaimTypes.Role, request.Login), new Claim(ClaimTypes.Name, request.Login)])]);
            await HttpContext.SignInAsync(principal);
            return Ok(principal.Claims);
        }
        return Unauthorized();
    }
}
