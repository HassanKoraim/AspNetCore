using ServiceConstracts;
using ServiceConstracts.DTO;
using Services;
using ServiceConstracts.Enums;
using Entities;
namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
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

        #region GetPersonByPersonId

        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            // Arrange
            Guid? personId = null;
            
            //Act 
           // PersonResponse? personResponse = _personsService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personId);
        }

        [Fact]
        public void GetPersonByPersonId_PersonIsNull()
        {
            //Arrange
            PersonResponse? personResponse = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => 
                //Act
                _personsService.GetPersonByPersonId(personResponse?.PersonId) 
            );
            
        }
        [Fact]
        public void GetPersonByPersonId_ProperPerson()
        {
            //Arrange 
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse =
                _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Email = "HassanKoriam2@gmail.com",
                DateOfBirth = Convert.ToDateTime("8-7-2000"),
                Gender = GenderOptions.male,
                Address = "Elwakf",
                ReceiveNewsLetter = true,
                CountryId = countryResponse.CountryId
            };
            PersonResponse? personResponse_from_add = _personsService.AddPerson(personRequest);

            //Act
            PersonResponse? personResponse_from_get = _personsService.GetPersonByPersonId(personResponse_from_add.PersonId);

            //Assert
            Assert.Equal(personResponse_from_get, personResponse_from_add);
        }
        #endregion
    }
}
