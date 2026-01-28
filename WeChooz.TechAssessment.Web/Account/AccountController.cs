using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace WeChooz.TechAssessment.Web.Account;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    [HttpGet("/Account/Login")]
    public ActionResult Login()
    {
        Response.Headers[HeaderNames.CacheControl] = "no-cache, must-revalidate";
        return View();
    }
}
