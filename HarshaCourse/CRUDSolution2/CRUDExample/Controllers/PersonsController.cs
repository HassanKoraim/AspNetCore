using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;
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
        [Route("/")]
        [Route("~/persons/index")]
        public IActionResult Index()
        {
            List<PersonResponse>? Persons = _personsService.GetAllPersons();
            return View(Persons);
        }
    }
}
