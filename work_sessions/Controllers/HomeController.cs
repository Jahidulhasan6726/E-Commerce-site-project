using Microsoft.AspNetCore.Mvc;

namespace work_sessions.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("r56", "web Application Development using C#.NET");
            return View();
        }
    }
}
