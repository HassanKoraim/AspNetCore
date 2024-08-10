using Microsoft.AspNetCore.Mvc;

namespace EnvironmentExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.CurrentEnvironment = _webHostEnvironment.EnvironmentName;
            return View();
        }
        
    }
}
