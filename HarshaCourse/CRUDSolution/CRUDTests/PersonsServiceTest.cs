using ServiceConstracts;
using ServiceConstracts.DTO;
using Services;
using ServiceConstracts.Enums;
using Entities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        //Constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            var CountriesIntialData = new List<Country>();
            var personsIntailData = new List<Person>();
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object ;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, CountriesIntialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons,personsIntailData);
            _countriesService = new CountriesService(dbContext);
            _personsService = new PersonsService(dbContext,_countriesService);
            _testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        // When supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            // Arrange 
            PersonAddRequest? personAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            // Act 
                await _personsService.AddPerson(personAddRequest)
            );
        }
        // When supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                //Act
                await _personsService.AddPerson(personAddRequest)
            );
        }
        [Fact]
        public async Task AddPerson_DuplicatePersonName()
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
            await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest1);
                await _personsService.AddPerson(personAddRequest2);
            });

        }

        // When we supply a proper Person details, it should be insert the Person into 
        // the Persons List; and it should return an object of PersonResponse, which 
        // include with the newly generated PersonId
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest {
                PersonName = "Hassan 52548", Email = "HassanKoraim2@gmail.com", Address = "Elwakf",
                CountryId = Guid.Parse("8F30BEDC-47DD-4286-8950-73D8A68E5D41"), Gender = GenderOptions.Male, ReceiveNewsLetter = true };

            //Act
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = await _personsService.GetAllPersons();

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
        public async Task GetPersonByPersonId_PersonIsNull()
        {
            //Arrange
            PersonResponse? personResponse = null;

            //Act
            PersonResponse? personResponse_from_get =
                        await _personsService.GetPersonByPersonId(personResponse?.PersonId);
            //Assert
            Assert.Null(personResponse_from_get);

        }
        [Fact]
        public async Task GetPersonByPersonId_ProperPerson()
        {
            //Arrange 
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse =
                await _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Email = "HassanKoriam2@gmail.com",
                DateOfBirth = Convert.ToDateTime("8-7-2000"),
                Gender = GenderOptions.Male,
                Address = "Elwakf",
                ReceiveNewsLetter = true,
                CountryId = countryResponse.CountryId
            };
            PersonResponse? personResponse_from_add = await _personsService.AddPerson(personRequest);

            //Act
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personResponse_from_add.PersonId);

            //Assert
            Assert.Equal(personResponse_from_get, personResponse_from_add);
        }
        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Arrange
            //   List<Person>? persons = null;

            //Act
            List<PersonResponse> person_response_from_get = await _personsService.GetAllPersons();

            Assert.Empty(person_response_from_get);

        }
        // First, We 

        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            List<PersonResponse> personsResponse_list_from_add = await createListOfPersonsResponse();
            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            //Act
            List<PersonResponse> personsResponse_list_from_get = await _personsService.GetAllPersons();

            //print personsResponse_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_get)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                //Assert
                Assert.Contains(personResponse_from_add, personsResponse_list_from_get);
            }
        }

        #endregion

        #region GetFilteredPersons
        private async Task<List<CountryResponse>> CreateListOfCountries()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Egypt" };
            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            return new List<CountryResponse> { countryResponse1, countryResponse2 };
        }
        private async Task<List<PersonResponse>> createListOfPersonsResponse()
        {
            //Arrange
            List<CountryResponse> countriesResponse_list = await CreateListOfCountries();

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Email = "Hassan@gmail.com",
                Address = "Address for hassan",
                // CountryId = countryResponse1.CountryId,
                CountryId = countriesResponse_list[0].CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("8-7-2000"),
                ReceiveNewsLetter = true,
            };
            //print the country id 
            //     _testOutputHelper.WriteLine($"Person 1: {personAddRequest1.CountryId}");
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Hussien",
                Email = "Hussien@gmail.com",
                Address = "Address for Hussien",
                //   CountryId = countryResponse2.CountryId,
                CountryId = countriesResponse_list[1].CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1-1-1990"),
                ReceiveNewsLetter = false,
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Fatma",
                Email = "Fatma@gmail.com",
                Address = "Address for Fatma",
                //CountryId = countryResponse2.CountryId,
                CountryId = countriesResponse_list[1].CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2005-5-8"),
                ReceiveNewsLetter = false,
            };
            List<PersonAddRequest> personsAddRequest = new List<PersonAddRequest>() {personAddRequest1,
            personAddRequest2, personAddRequest3};

            List<PersonResponse> personsResponse_list_from_add = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personsAddRequest)
            {
                PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
                personsResponse_list_from_add.Add(personResponse);
            }
            return personsResponse_list_from_add;
        }
        //if the search text is empty and the searchBy is "PersonName",
        //it should return all Persons
        [Fact]
        public async Task GetFilteredPersons_AllPersons()
        {
            //Arrange
            List < PersonResponse> personsResponse_list_from_add = await createListOfPersonsResponse();

            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            //Act
            List<PersonResponse> personsResponse_list_from_search =
                await _personsService.GetFilteredPersons(nameof(Person.PersonName),"");

            //print personsResponse_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_search)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                //Assert
                Assert.Contains(personResponse_from_add, personsResponse_list_from_search);
            }
        }
        //First we will add few Persons; and then we will search based on person name 
        // with some search string, it should be return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            List<PersonResponse> personsResponse_list_from_add = await createListOfPersonsResponse();

            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            //Act
            List<PersonResponse> personsResponse_list_from_search =
                await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //print personsResponse_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_search)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                if (personResponse_from_add != null)
                {
                    if (personResponse_from_add.PersonName.Contains(
                        "ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse_from_add, personsResponse_list_from_search);
                    }
                }
            }
        }
        // That's I make it 
        public async Task GetFilteredPersons_NullPara()
        {
            //Arrange
            string? SearchBy = null;
            string? SearchString = null;
            
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    //Act
                     await _personsService.GetFilteredPersons(SearchBy, SearchString)
            );
        }

        #endregion

        #region GetSortedPersons

        //when we sort based on PersonName in DESC, it should return Persons list in descending 
        // on PersonName 
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            List<PersonResponse> personsResponse_list_from_add = await createListOfPersonsResponse();

            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = await _personsService.GetAllPersons();
            List<PersonResponse> personsResponse_list_from_sort =
                await _personsService.GetSortedPersons(allPersons,
                nameof(PersonResponse.Age), SortOrderOptions.ASC);

            //print personsResponse_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_sort)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            personsResponse_list_from_add =
                personsResponse_list_from_add.OrderBy(temp => temp.Age).ToList();

            for (int i = 0; i < personsResponse_list_from_add.Count; i++)
            {
                //Assert
                Assert.Equal(personsResponse_list_from_add[i], personsResponse_list_from_sort[i]);
            }


        }

        #endregion

        #region UpdatePersonName


        // when PersonUpdateRequest is null,it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePersonName_NullPersonUpdateRequest()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act 
                await _personsService.UpdatePerson(personUpdateRequest)
            );

        }


        // when we supply person id is invalid, it should throw ArgumentException
        [Fact]
        public async Task UpdatePersonName_invalidPersonId()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid()
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act 
              await _personsService.UpdatePerson(personUpdateRequest)
            );
        }

        // when PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePersonName_NullPersonName()
        {
            //Arrange
            List<PersonResponse> personsResponse_from_create = await createListOfPersonsResponse();

            PersonUpdateRequest personUpdateRequest = personsResponse_from_create[0].ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act 
                await _personsService.UpdatePerson(personUpdateRequest)
            );  
        }

        // First, add new Person and try to update the Person name and Email
        [Fact] 
        public async Task UpdatePersonName_PersonFullDetailsUpdation()
        {
            //Arrange

            // The List of Persons Response objects that we create them 
            List<PersonResponse> personsResponse_to_create = await createListOfPersonsResponse();

            // Convert the personResponse that is the first object in list, to PersonUpdateRequest object
            PersonUpdateRequest personUpdateRequest = personsResponse_to_create[0].ToPersonUpdateRequest();
            // Change the PersonName and Email
            personUpdateRequest.PersonName = "Ibrahim";
            personUpdateRequest.Email = "Ibrahim@gmail.com";

            //Act
            PersonResponse? personRespons_from_update = 
                await _personsService.UpdatePerson(personUpdateRequest);
            //Actual
            _testOutputHelper.WriteLine($"Actual : {personRespons_from_update?.ToString()}");
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
            //Excepting
            _testOutputHelper.WriteLine($"Excepting : {personResponse_from_get?.ToString()}");
            //Assert
            Assert.Equal(personResponse_from_get, personRespons_from_update);

        }


        #endregion

        #region DeletePerson

        // if Supply PersonDeleteRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task DeletePerson_personIdIsNull()
        {
            //Arrange
            Guid? personId = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async ()=>
            // Act
               await _personsService.DeletePerson(personId)
            );

        }

        // if supply invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            //Arrange

            Guid? personId = Guid.NewGuid();

            //Assert 
            Assert.False(await _personsService.DeletePerson(personId));
        }

        [Fact]
        public async Task DeletePerson_ValidPerson() 
        {
            //Arragne
            List<PersonResponse>? personsResponse_list = await createListOfPersonsResponse();
            PersonResponse person_response_from_add = personsResponse_list[0];

            // Act 
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonId);

            // Assert
            Assert.Null(await _personsService.GetPersonByPersonId(person_response_from_add.PersonId));
            Assert.True(isDeleted);
        }
        #endregion

    }
}
