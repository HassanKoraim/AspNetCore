using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        public PersonsService()
        {
            _persons = new List<Person>() { };
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
                throw new ArgumentNullException();
            if(personAddRequest.PersonName == null) throw new ArgumentException();
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
            return person.ToPersonResponse();
            
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
    }
}
