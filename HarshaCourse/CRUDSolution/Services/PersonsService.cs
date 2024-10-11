using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;
        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }
        private PersonResponse ConvertPersonIntoPersonResponse(Person? person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //Validta: if PersonAddRequest is null
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            //Model Validations 
            ValidationHelper.ModelValidation(personAddRequest);
            //Validate: if Perosn Name is already exists
            if(_persons.Where(temp => temp.PersonName == personAddRequest.PersonName).Count() > 0)
            {
                throw new ArgumentException("Given Person Name is already exists");
            }
            //Convert object from PersonAddRequest to Person type
            Person person = personAddRequest.ToPerson();
            // generate PersonId 
            person.PersonId = Guid.NewGuid();
            // Add to _Persons List
            _persons.Add(person);
            return ConvertPersonIntoPersonResponse(person);     
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(string? personName)
        {
            throw new NotImplementedException();
        }

        public PersonResponse? UpdatePersonName(string? personName)
        {
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("Person Id Can't be blank");
            Person? person = _persons.Where(temp => temp.PersonId == personId).FirstOrDefault();
            PersonResponse? personResponse = ConvertPersonIntoPersonResponse(person);
            ValidationHelper.ModelValidation(person);
            return personResponse;
        }
    }
}
