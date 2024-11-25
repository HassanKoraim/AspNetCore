using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;
using Services;
using ServiceConstracts.DTO;
using ServiceConstracts.Enums;
using System.Collections.Generic;

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
        public IActionResult Index (string searchBy, string? searchString, string? sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.Address), "Address" },
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.Age), "Age" }
            };
            List<PersonResponse> AllPersons = _personsService.GetFilteredPersons(searchBy,searchString);
            ViewBag.currentSearchBy = searchBy;
            ViewBag.currentSearchString = searchString;
            return View(AllPersons);
        }
    }
}
