using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;
using Services;
using ServiceConstracts.DTO;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index ()
        {
            
            List<PersonResponse> AllPersons = _personsService.GetAllPersons();
            return View(AllPersons);
        }
    }
}
