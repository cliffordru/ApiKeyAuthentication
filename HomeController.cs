using ApiKeyAuthentication.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeyAuthentication
{
    public class HomeController : Controller
    {
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [HttpGet("protected")]
        //[ApiKeyAuthFilter]
        public IActionResult Index()
        {
            return Content("Welcome to protected home!");
        }


        [HttpGet("unprotected")]
        public IActionResult Unprotected()
        {
            return Content("Welcome to unprotected home!");
        }
    }
}
