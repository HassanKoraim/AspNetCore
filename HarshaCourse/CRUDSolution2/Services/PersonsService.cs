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
        public PersonsService(ICountriesService countriesService)
        {
            _countriesService = countriesService ?? throw new ArgumentNullException(nameof(countriesService));
            _persons = new List<Person>();
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
            _persons.Add(person);
            PersonResponse? personResponse = person.ToPersonResponse();
            return personResponse;
        }

        public bool DeletePerson(Guid? personId)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponse>? GetAllPersons()
        {
            List<PersonResponse>? personsResponse 
                     = _persons?.Select(person => person.ToPersonResponse()).ToList();
            return personsResponse;
        }

        public List<PersonResponse> GetFilterdPersons(string? searchBy, string? searchString)
        {
            if(string.IsNullOrEmpty(searchBy)) throw new ArgumentNullException(nameof(searchString));
            List<PersonResponse> FilteredPersons = (searchBy) switch
            {
                "PersonName" => _persons.
                Where(temp => string.Compare(temp.PersonName, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()).ToList(),

                "Email" => _persons.
                Where(temp => string.Compare(temp.Email, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()).ToList(),

                "Gender" => _persons.
                Where(temp => string.Compare(temp.Gender, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()).ToList(),

                "BirthDate" => _persons.
                Where(temp => temp.BirthDate.ToString() == searchString)
                .Select(temp => temp.ToPersonResponse()).ToList(),

                "Address" => _persons.
                Where(temp => string.Compare(temp.Address, searchString, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(temp => temp.ToPersonResponse()).ToList(),
                _ => _persons?.Select(person => person.ToPersonResponse()).ToList()
            };
            return FilteredPersons;
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId == null) throw new ArgumentNullException("Person Id can't be blank",nameof(personId));
            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (person == null) throw new ArgumentException("Given Person not valid");
            return person.ToPersonResponse();

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string? sortBy, sortOrderOption? sortOrder)
        {
            throw new NotImplementedException();
        }

        public PersonResponse UpdatePerson(Guid? personId)
        {
            throw new NotImplementedException();
        }
    }
}
