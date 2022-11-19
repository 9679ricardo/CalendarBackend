using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}