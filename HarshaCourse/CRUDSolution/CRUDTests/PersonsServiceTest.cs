using ServiceConstracts;
using ServiceConstracts.DTO;
using Services;
using ServiceConstracts.Enums;
namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
        }
        #region AddPerson
        // When supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange 
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            // Act 
            _personsService.AddPerson(personAddRequest)
            );
        }
        // When supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null};

             //Assert
             Assert.Throws<ArgumentException>(() =>
                  //Act
                 _personsService.AddPerson(personAddRequest)
             );
        }
        [Fact]
        public void AddPerson_DuplicatePersonName() 
        {
            //Arrange
            PersonAddRequest personAddRequest1 = new PersonAddRequest() {
                PersonName = "Hassan", Email = "Hassankoraim2@gmail.com" };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Email = "Hassankoraim2@gmail.com"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest1);
                _personsService.AddPerson(personAddRequest2);
            });
             
        }

        // When we supply a proper Person details, it should be insert the Person into 
        // the Persons List; and it should return an object of PersonResponse, which 
        // include with the newly generated PersonId
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest {
                PersonName = "Hassan", Email = "HassanKoraim2@gmail.com" , Address = "Elwakf",
                Gender = GenderOptions.male, ReceiveNewsLetter = true};

            //Act
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, persons_list);
        }
        #endregion
    }
}
