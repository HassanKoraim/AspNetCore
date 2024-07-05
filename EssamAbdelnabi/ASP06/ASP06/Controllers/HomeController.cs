using Microsoft.AspNetCore.Mvc;

namespace ControllerApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
