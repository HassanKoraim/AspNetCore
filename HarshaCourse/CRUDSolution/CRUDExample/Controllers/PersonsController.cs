using Microsoft.AspNetCore.Mvc;
using ServiceConstracts;
using Services;
using ServiceConstracts.DTO;
using ServiceConstracts.Enums;
using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
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
        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string? sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
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
            List<PersonResponse> AllPersons = await _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.currentSearchBy = searchBy;
            ViewBag.currentSearchString = searchString;

            //Sort
            List<PersonResponse> SortedPersons = await _personsService.GetSortedPersons(AllPersons, sortBy, sortOrder);
            ViewBag.currentSortBy = sortBy;
            ViewBag.currentSortOrder = sortOrder.ToString();
            return View(SortedPersons);
        }

        // Excutes when the users clicks on "Create Person" hyperlink (while openning the create view)     
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }
            );
            // new SelectListItem { Text = "Hassan", Value = "1" };
            // <option value = 1> Hassan <option/>
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PersonAddRequest personAddRequest)
        {
            // this is before I use Helper tags and Jquery Validation 
            /*if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }
                            );
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }*/

            // call the service method
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            // navigate to Index() action method (it makes another get request to "persons/index")   
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{PersonID}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid PersonID)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(PersonID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }
            PersonUpdateRequest? personUpdateRequest = personResponse?.ToPersonUpdateRequest();
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }
            );
            // new SelectListItem { Text = "Hassan", Value = "1" };
            // <option value = 1> Hassan <option/>
            return View(personUpdateRequest);
        }

        [Route("[action]/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                return RedirectToAction("Index");
            }
            await _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{PersonId}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid personId) 
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(personResponse);
        }

        [Route("[action]/{PersonId}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = 
                await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if(personResponse == null)
                return RedirectToAction("Index");
            // call Delete method on service
            await _personsService.DeletePerson(personUpdateRequest.PersonId);
            return RedirectToAction("Index", "Persons");
        }
        [Route("PersonPdf")]
        public async Task<IActionResult> PersonsPdf(string persons)
        {
            // List<PersonResponse> persons = await _personsService.GetAllPersons();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var personList = JsonSerializer.Deserialize<List<PersonResponse>>(persons, options);
            return new ViewAsPdf("PersonsPdf", personList, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Left = 20,
                    Bottom = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV(string persons)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var personList = JsonSerializer.Deserialize<List<PersonResponse>>(persons, options);
            MemoryStream memoryStream = await _personsService.GetPersonsCSV(personList);
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }
        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel(string persons)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var personList = JsonSerializer.Deserialize<List<PersonResponse>>(persons, options);
            MemoryStream memoryStream = await _personsService.GetPersonsExcel(personList);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }

    }
}
