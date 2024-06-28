using Microsoft.AspNetCore.Mvc;
//using Services;
using ServiceContracts;
namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //private ICitiesService _citiesService;

        //Constructor
        public HomeController()
        {
            // Create object of citiesService class
            // _citiesService = citiesService; //new CitiesService();
        }
        [Route("/")]
        public IActionResult Index([FromServices] ICitiesService citiesService)
        {
            // _citiesService = citiesService;
            List<string> cities = citiesService.getCities();
            return View(cities);
        }
    }
}