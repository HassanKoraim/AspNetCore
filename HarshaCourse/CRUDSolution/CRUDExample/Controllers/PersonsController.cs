using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;
using Services;
using ServiceConstracts.DTO;
using ServiceConstracts.Enums;
using System.Collections.Generic;
using Entities;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        // private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
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
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Age), "Age" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.country), "Country" },
                {nameof(PersonResponse.Address), "Address" },
                {nameof(PersonResponse.ReceiveNewsLetter), "Receive News Letter" },
            };
            List<PersonResponse> AllPersons = _personsService.GetFilteredPersons(searchBy,searchString);
            ViewBag.currentSearchBy = searchBy;
            ViewBag.currentSearchString = searchString;

            //Sort
            List <PersonResponse> SortedPersons = _personsService.GetSortedPersons(AllPersons, sortBy, sortOrder);
            ViewBag.currentSortBy = sortBy;
            ViewBag.currentSortOrder = sortOrder.ToString();
            return View(SortedPersons);
        }

        // Excutes when the users clicks on "Create Person" hyperlink (while openning the create view)     
        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("persons/create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid) 
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            // call the service method
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            // navigate to Index() action method (it makes another get request to "persons/index")   
            return RedirectToAction("Index","Persons");
        }

        [Route("persons/edit")]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(id);
            return View(personResponse);
        }

        [Route("persons/edit")]
        [HttpPost]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index","Persons");
        }

        [Route("persons/delete")]
        [HttpPost]
        public IActionResult Delete(Guid id)
        { 
            // call Delete method on service
            _personsService.DeletePerson(id);
            return RedirectToAction("Index","Persons");
        }

    }
}
