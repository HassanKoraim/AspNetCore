using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Services.Helpers;
using ServiceConstracts.Enums;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;
        public PersonsService(PersonsDbContext personsDbContext , ICountriesService countriesService)
        {
            _db = personsDbContext;
            _countriesService = countriesService;
        }
        /*private PersonResponse? ConvertPersonIntoPersonResponse(Person? person)
        {
            PersonResponse? personResponse = person?.ToPersonResponse();
            personResponse.country = person?.Country?.CountryName;
            return personResponse;
        }*/
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //Validta: if PersonAddRequest is null
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            //Model Validations 
            ValidationHelper.ModelValidation(personAddRequest);
            //Validate: if Perosn Name is already exists
            if(_db.Persons.Where(temp => temp.PersonName == personAddRequest.PersonName).Count() > 0)
            {
                throw new ArgumentException("Given Person Name is already exists");
            }
            //Convert object from PersonAddRequest to Person type
            Person person = personAddRequest.ToPerson();
            // generate PersonId 
            person.PersonId = Guid.NewGuid();
            // Add to _Persons List
          /*  _db.Persons.Add(person);
            _db.SaveChanges. */
          _db.sp_InsertPerson(person);

            return person.ToPersonResponse();     
        }
        /// <summary>
        /// Returns All Persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse Type</returns>
        public List<PersonResponse> GetAllPersons()
        {
            var persons = _db.Persons.Include(nameof(Country)).ToList();
            return persons.   
                Select(temp => temp.ToPersonResponse()).ToList();
            /*return _db.sp_GetAllPersons().
                Select(temp => temp.ToPersonResponse()).ToList();*/
        }
        
        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) return null;
            Person? person = _db.Persons.Include(nameof(Country)).
                FirstOrDefault(temp => temp.PersonId == personId);
            if (person == null) return null;
            PersonResponse? personResponse = person.ToPersonResponse();
            return personResponse;
        }

        public List<PersonResponse>? GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<PersonResponse>? allPersons = GetAllPersons();
            List<PersonResponse>? matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(
                        temp => !string.IsNullOrEmpty(temp.PersonName)? 
                        temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase):true).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(
                        temp => !string.IsNullOrEmpty(temp.Email) ?
                        temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(
                        temp => temp.DateOfBirth != null ?
                        temp.DateOfBirth.Value.ToString("dd MMMM YYYY").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(
                        temp => !string.IsNullOrEmpty(temp.Gender) ?
                        temp.Gender == searchString : true).ToList();
                    break;
                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(
                        temp => !string.IsNullOrEmpty(temp.country) ?
                        temp.country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(
                        temp => !string.IsNullOrEmpty(temp.Address) ?
                        temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Age):
                    matchingPersons = allPersons.Where(
                        temp => (temp.Age != null) ?
                        temp.Age == Convert.ToDouble(searchString) : true).ToList();
                    break;
                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                //When sorting by Person Name
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                //When sorting by Email
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                //When sorting by Date of Birth
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                //When sorting by Age
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Age).ToList(),

                //When sorting by Gender
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                //When sorting by Country
                (nameof(PersonResponse.country), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.country), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.country, StringComparer.OrdinalIgnoreCase).ToList(),

                //When sorting by Address
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                //When sorting by ReceiveNewsLetter
                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.ReceiveNewsLetter).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.ReceiveNewsLetter).ToList(),

                // defualt case
                _ => allPersons
            };
            return sortedPersons;
            
        }

        public PersonResponse? UpdatePerson(PersonUpdateRequest? updatePersonRequest)
        {
            // if we supply updatePersonRequest null, it should throw ArgumentNullException
            if(updatePersonRequest == null) throw new ArgumentNullException(nameof(updatePersonRequest));
            /*
            // if we supply PersonName null, it should throw ArgumentException
            if (updatePersonRequest?.PersonName == null) throw new ArgumentException(nameof(updatePersonRequest.PersonName));
            // if we supply PersonId null, it should throw ArgumentException
            if (updatePersonRequest?.PersonId == null) throw new ArgumentException(nameof(updatePersonRequest.PersonId));
            // if we supply PersonId is invalid, it should throw ArgumentException
            */

            //Validation
            ValidationHelper.ModelValidation(updatePersonRequest);
            // Get matching Person object to UpdatePersonRequest
             Person? matchingPerson = _db.Persons.FirstOrDefault(temp => temp.PersonId == updatePersonRequest.PersonId);
            if (matchingPerson == null) throw new ArgumentException("Given Person Id Not exist");
            //Update all
            matchingPerson.PersonName = updatePersonRequest.PersonName;
            matchingPerson.Email = updatePersonRequest.Email;
            matchingPerson.DateOfBirth = updatePersonRequest.DateOfBirth;
            matchingPerson.Address = updatePersonRequest.Address;
            matchingPerson.CountryId = updatePersonRequest.CountryId;
            matchingPerson.Gender = updatePersonRequest.Gender.ToString();
            matchingPerson.ReceiveNewsLetter = updatePersonRequest.ReceiveNewsLetter;
            _db.SaveChanges(); //UPDATE
            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personId)
        {

            if(personId == null) throw new ArgumentNullException();

            Person? matchingPerson = _db.Persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (matchingPerson == null)  return false;
            /*_db.Persons.Remove(_db.Persons.First(temp => temp.PersonId == personId));
            _db.SaveChanges(); //DELETE */
            _db.sp_DeletePerson(personId);
            return true;

        }
    }
}
