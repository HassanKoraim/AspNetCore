using ServiceConstracts;
using ServiceConstracts.DTO;
using ServiceConstracts.Enums;
using Services.Helper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly ICountriesService _countriesService;
        private readonly List<Person>? _persons;
        public PersonsService(ICountriesService countriesService, bool init = true)
        {
            _countriesService = countriesService ?? throw new ArgumentNullException(nameof(countriesService));
            _persons = new List<Person>();
            if (init)
            {
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("E81E31EA-D85B-4F00-B794-DD8B1B720F4D"),
                    PersonName = "Sandi",
                    Email = "smilmoe0@abc.net.au",
                    DateOfBirth = DateTime.Parse("1995-03-03"),
                    Gender = "Female",
                    Address = "2696 Elka Pass",
                    ReceiveNewsLetter = true,
                    CountryId = Guid.Parse("A81AD7BA-96B6-40F8-817B-1FB7AA02B89F")
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("2FB64DD1-7104-4D13-9D9F-EC8456782CD3"),
                    PersonName = "Darn",
                    Email = "dlevensky1@indiegogo.com",
                    DateOfBirth = DateTime.Parse("1992-08-24"),
                    Gender = "Male",
                    Address = "89 Loftsgordon Street",
                    ReceiveNewsLetter = false,
                    CountryId = Guid.Parse("C01A78A7-98CE-41C1-8D52-B4279120F9EF")
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("0E3327DB-0270-45E3-B12D-87264BA72A82"),
                    PersonName = "Angil",
                    Email = "athurborn2@hugedomains.com",
                    DateOfBirth = DateTime.Parse("1999-04-04"),
                    Gender = "Female",
                    Address = "46 Prairie Rose Avenue",
                    ReceiveNewsLetter = false,
                    CountryId = Guid.Parse("A81AD7BA-96B6-40F8-817B-1FB7AA02B89F")
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("CD8B716F-F3A9-444B-8E0C-F91753D0A6BC"),
                    PersonName = "Amandie",
                    Email = "aramsden3@networkadvertising.org",
                    DateOfBirth = DateTime.Parse("2000-04-27"),
                    Gender = "Female",
                    Address = "9440 Summit Street",
                    ReceiveNewsLetter = false,
                    CountryId = Guid.Parse("8E9D4111-1C27-41E0-AAFB-360A11887FBC")
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("F652A57F-CEC0-4EDA-A46D-AEB2DD7732FB"),
                    PersonName = "Rusty",
                    Email = "rmilthorpe4@vimeo.com",
                    DateOfBirth = DateTime.Parse("2000-05-10"),
                    Gender = "Male",
                    Address = "2588 Lighthouse Bay Court",
                    ReceiveNewsLetter = true,
                    CountryId = Guid.Parse("A4E6C7A2-8306-4116-B9FA-3791CC2B0529")
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("EB61B0C0-EF3E-4401-930C-5A8EDBC54738"),
                    PersonName = "Portia",
                    Email = "ppitkin5@posterous.com",
                    DateOfBirth = DateTime.Parse("1999-09-18"),
                    Gender = "Female",
                    Address = "2 Hoepker Parkway",
                    ReceiveNewsLetter = true,
                    CountryId = Guid.Parse("7099A3F2-9820-4EC0-BCF5-ECB968BD516F")
                });
            }
        }
        private PersonResponse ConvertPersonIntoPersonResponse(Person? person)
        {
            PersonResponse? personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countriesService.GetCountryByCountryId(person?.CountryId)?.CountryName;
            return personResponse;
        }
        public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
        {
            // check if personAddRequest is null, if true throw ArgumentnNullException
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            // check if {PersonName , CountryId, Email} is null and Invalid Email is true,
            // throw ArgumentException
            ValidationHelper.ModelValidation(personAddRequest);
            // check if the countryId is valid
            CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(personAddRequest.CountryId);
            if (countryResponse == null) throw new ArgumentException("Given valid Country id");
            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons?.Add(person);
            PersonResponse? personResponse = ConvertPersonIntoPersonResponse(person);
            return personResponse;
        }

        public bool DeletePerson(Guid? personId)
        {
            if(_persons == null) return true;
            // when we supply null Guid, it should throw ArgumentNullException
           // if (personId == null) throw new ArgumentNullException("The Person Id Can't be null");
            PersonResponse? personReponse = GetPersonByPersonId(personId);
            // when we supply Guid doesn't Exist in the Persons,
            // it should throw ArgumentException with wrong message
            if (personReponse == null) throw new ArgumentException("The Person Id doesnt Exist");
            Person person = _persons.Single(p => p.PersonId == personId);
            _persons.Remove(person);
            return true;
        }

        public List<PersonResponse>? GetAllPersons()
        {
            List<PersonResponse>? personsResponse 
                     = _persons?.Select(person => ConvertPersonIntoPersonResponse(person)!).ToList();
            return personsResponse;
        }

        public List<PersonResponse> GetFilterdPersons(string? searchBy, string? searchString)
        {
            // if SerchBy is null, it should be throw ArgumentNullException
            if(string.IsNullOrEmpty(searchBy)) throw new ArgumentNullException(nameof(searchBy));
            if (string.IsNullOrEmpty(searchString))
            {
                return _persons?.Select(person => person.ToPersonResponse()!).ToList()?? new List<PersonResponse>();
            }
            List<PersonResponse>? FilteredPersons = (searchBy) switch
            {
                "PersonName" => _persons?.
                Where(temp => string.Compare(temp.PersonName, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()!).ToList()?? new List<PersonResponse>(),

                "Email" => _persons?.
                Where(temp => string.Compare(temp.Email, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()!).ToList(),

                "Gender" => _persons?.
                Where(temp => string.Compare(temp.Gender, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()!).ToList(),

                "BirthDate" => _persons?.
                Where(temp => temp.DateOfBirth.ToString() == searchString)
                .Select(temp => temp.ToPersonResponse()!).ToList(),

                "Address" => _persons?.
                Where(temp => string.Compare(temp.Address, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()!).ToList(),
                _ => _persons?.Select(person => person.ToPersonResponse()!).ToList()
            };
            return FilteredPersons?? new List<PersonResponse>();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId == null) throw new ArgumentNullException("Person Id can't be blank",nameof(personId));
            Person? person = _persons?.FirstOrDefault(temp => temp.PersonId == personId);
            if (person == null) throw new ArgumentException("Given Person not valid");
            return person.ToPersonResponse();

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse>? allpersons, string? sortBy, sortOrderOption? sortOrder)
        { 
            // if we supply null List, it should throw ArrgumentNullException
            if (allpersons == null) throw new ArgumentNullException(nameof(allpersons));
            // if supply null value in SortyBy, it should return all List without arrangement
            if (string.IsNullOrEmpty(nameof(sortBy))) return allpersons;
            List<PersonResponse>? SortedList = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonId), sortOrderOption.ASC) =>
                   allpersons.OrderBy(temp => temp.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonId), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), sortOrderOption.ASC) =>
                    allpersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), sortOrderOption.ASC) =>
                    allpersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), sortOrderOption.ASC) =>
                    allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Address), sortOrderOption.ASC) =>
                   allpersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), sortOrderOption.ASC) =>
                   allpersons.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), sortOrderOption.DESC) =>
                allpersons.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allpersons
            };
            return SortedList;
        }

        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));
            //Validate
            ValidationHelper.ModelValidation(personUpdateRequest);
            // check if The person is existing or not
            Person? matchingPerson = _persons?.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
            if (matchingPerson == null)
            {
                throw new ArgumentException(nameof(matchingPerson), "This Perosn doesnot exist");
            }
            matchingPerson.PersonId = personUpdateRequest.PersonId;
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.DateOfBirth = personUpdateRequest.BirthDate;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.ReceiveNewsLetter = personUpdateRequest.ReciveLetter;
            return matchingPerson?.ToPersonResponse();
        }
    }
}
