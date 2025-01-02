using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Services.Helpers;
using ServiceConstracts.Enums;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _persons.Add(new Person() { PersonId = Guid.Parse("E81E31EA-D85B-4F00-B794-DD8B1B720F4D"),
                    PersonName = "Sandi", Email = "smilmoe0@abc.net.au", DateOfBirth = DateTime.Parse("1995-03-03"),
                    Gender = "Female", Address = "2696 Elka Pass",
                    ReceiveNewsLetter = true, CountryId = Guid.Parse("A81AD7BA-96B6-40F8-817B-1FB7AA02B89F")
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
                _persons.Add(new Person() { PersonId = Guid.Parse("0E3327DB-0270-45E3-B12D-87264BA72A82"),
                    PersonName = "Angil", Email = "athurborn2@hugedomains.com", DateOfBirth = DateTime.Parse("1999-04-04"),
                    Gender = "Female", Address = "46 Prairie Rose Avenue",
                    ReceiveNewsLetter = false, CountryId = Guid.Parse("A81AD7BA-96B6-40F8-817B-1FB7AA02B89F")
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
                // {
                /*
,,,,,true
Sebastian,sgoldston6@wikia.com,1991-10-20,Male,0 Chive Crossing,false
Amandie,amccullough7@comsenz.com,1995-03-01,Female,73533 Fuller Park,true
Ainslie,acareless8@rakuten.co.jp,2000-12-10,Female,017 Granby Junction,false
Kessia,kcomar9@google.nl,1990-10-21,Female,020 Florence Park,false
                */
            }
        }
        private PersonResponse? ConvertPersonIntoPersonResponse(Person? person)
        {
            PersonResponse? personResponse = person?.ToPersonResponse();
            personResponse.country = _countriesService.GetCountryByCountryId(person?.CountryId)?.CountryName;
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
        /// <summary>
        /// Returns All Persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse Type</returns>
        public List<PersonResponse>? GetAllPersons()
        {
            return _persons?.Select(temp => ConvertPersonIntoPersonResponse(temp)).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) return null;
            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (person == null) return null;
            PersonResponse? personResponse = ConvertPersonIntoPersonResponse(person);
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
             Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == updatePersonRequest.PersonId);
            if (matchingPerson == null) throw new ArgumentException("Given Person Id Not exist");
            //Update all
            matchingPerson.PersonName = updatePersonRequest.PersonName;
            matchingPerson.Email = updatePersonRequest.Email;
            matchingPerson.DateOfBirth = updatePersonRequest.DateOfBirth;
            matchingPerson.Address = updatePersonRequest.Address;
            matchingPerson.CountryId = updatePersonRequest.CountryId;
            matchingPerson.Gender = updatePersonRequest.Gender.ToString();
            matchingPerson.ReceiveNewsLetter = updatePersonRequest.ReceiveNewsLetter;
            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personId)
        {

            if(personId == null) throw new ArgumentNullException();

            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (matchingPerson == null)  return false;

            _persons.RemoveAll(temp => temp.PersonId == personId);
            return true;

        }
    }
}
