using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DOB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }


        [Authorize(Policy = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        //will not do any authorization
        [AllowAnonymous]
        public IActionResult Authenticate()
        {

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Keanu"),
                new Claim(ClaimTypes.Email, "rKeanu@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "12/12/2000"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Grandma Says", "Keanu is great"),
            };


            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Keanu Reaves"),
                new Claim("DrivingLicense", "Valid"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government Identity");

            var userPrinicipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrinicipal);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff(
             [FromServices] IAuthorizationService authorizationService)
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();
            var authResult = await _authorizationService.AuthorizeAsync(HttpContext.User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");
            }

            return View("Index");
        }
    }
}
