using Microsoft.AspNetCore.Mvc;

namespace work_sessions.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
