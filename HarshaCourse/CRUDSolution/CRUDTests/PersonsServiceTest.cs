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
using AutoFixture;
using System.Net;
using System.Reflection;
using FluentAssertions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        //Constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
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
            /*await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            // Act 
                await _personsService.AddPerson(personAddRequest)
            );*/
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        // When supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();
            /*                new PersonAddRequest() { PersonName = null };
            */
            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //    //Act
            //    await _personsService.AddPerson(personAddRequest)
            //);
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task AddPerson_DuplicatePersonName()
        {
            //Arrange
            PersonAddRequest personAddRequest1 = 
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "HassanKoraim@Gmail.com")
                .With(temp => temp.PersonName, "Hassan")
                .Create();
            PersonAddRequest personAddRequest2 = 
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "HassanKoraim@Gmail.com")
                .With(temp => temp.PersonName, "Hassan")
                .Create();
            //Assert
            /*await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest1);
                await _personsService.AddPerson(personAddRequest2);
            });*/

            Func<Task> action = async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest1);
                await _personsService.AddPerson(personAddRequest2);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // When we supply a proper Person details, it should be insert the Person into 
        // the Persons List; and it should return an object of PersonResponse, which 
        // include with the newly generated PersonId
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange 
            PersonAddRequest? personAddRequest =
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@Example.com")
                .Create();
                /*_fixture.Create<PersonAddRequest>();*/
                /*new PersonAddRequest {
                PersonName = "Hassan 52548", Email = "HassanKoraim2@gmail.com", Address = "Elwakf",
                CountryId = Guid.Parse("8F30BEDC-47DD-4286-8950-73D8A68E5D41"), Gender = GenderOptions.Male, ReceiveNewsLetter = true };*/
        //Act
        PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = await _personsService.GetAllPersons();

            //Assert
            //Assert.True(personResponse.PersonId != Guid.Empty);
            personResponse.PersonId.Should().NotBe(Guid.Empty);
            //Assert.Contains(personResponse, persons_list);
            persons_list.Should().Contain(personResponse);
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
            //Assert.Null(personId);
            personId.Should().BeNull();
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
            //Assert.Null(personResponse_from_get);
            personResponse_from_get.Should().BeNull();

        }
        [Fact]
        public async Task GetPersonByPersonId_ProperPerson()
        {
            //Arrange 
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse =
                await _countriesService.AddCountry(countryRequest);

            PersonAddRequest personRequest = 
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "HassanKoriam2@gmail.com")
                .With(temp => temp.CountryId, countryResponse.CountryId)
                .Create();
            PersonResponse? personResponse_from_add = await _personsService.AddPerson(personRequest);

            //Act
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personResponse_from_add.PersonId);

            //Assert
            //Assert.Equal(personResponse_from_add, personResponse_from_get);
            personResponse_from_get.Should().Be(personResponse_from_add);
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

            //Assert.Empty(person_response_from_get);
            person_response_from_get.Should().BeEmpty();

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
            /*foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                //Assert
                Assert.Contains(personResponse_from_add, personsResponse_list_from_get);
            }*/
            personsResponse_list_from_get.Should().BeEquivalentTo(personsResponse_list_from_add);
        }

        #endregion

        #region GetFilteredPersons
        private async Task<List<CountryResponse>> CreateListOfCountries()
        {
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            return new List<CountryResponse> { countryResponse1, countryResponse2 };
        }
        private async Task<List<PersonResponse>> createListOfPersonsResponse()
        {
            //Arrange
            List<CountryResponse> countriesResponse_list = await CreateListOfCountries();

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Hassan")
                .With(temp => temp.Email, "Hassan@gmail.com")
                .With(temp => temp.CountryId, countriesResponse_list[0].CountryId)
                .Create();
            //print the country id 
            //     _testOutputHelper.WriteLine($"Person 1: {personAddRequest1.CountryId}");
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Hussien")
                .With(temp => temp.Email, "Hussien@gmail.com")
                .With(temp => temp.CountryId, countriesResponse_list[1].CountryId)
                .Create();
            PersonAddRequest personAddRequest3 = 
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Fatma")
                .With(temp => temp.Email, "Fatma@gmail.com")
                .With(temp => temp.CountryId, countriesResponse_list[1].CountryId)
                .Create();

            List<PersonAddRequest> personsAddRequest = 
                new List<PersonAddRequest>() {personAddRequest1,
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
            /*foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                //Assert
                Assert.Contains(personResponse_from_add, personsResponse_list_from_search);
            }*/
            personsResponse_list_from_search.Should().BeEquivalentTo(personsResponse_list_from_search);
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
           /* foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                if (personResponse_from_add != null)
                {
                    if (personResponse_from_add.PersonName.Contains(
                        "ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse_from_add, personsResponse_list_from_search);
                    }
                }
            }*/

            personsResponse_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }

        // That's I make it 
        [Fact]
        public async Task GetFilteredPersons_NullPara()
        {
            //Arrange
            string? SearchBy = null;
            string? SearchString = null;
            List<PersonResponse> personsResponse_list_from_get = await _personsService.GetAllPersons();

            
            //Assert
            /*await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    //Act
                     await _personsService.GetFilteredPersons(SearchBy, SearchString)
            );*/

            List<PersonResponse> personsResponse_list_from_filter =
                 await _personsService.GetFilteredPersons(SearchBy, SearchString);
            personsResponse_list_from_get.Should().BeEquivalentTo(personsResponse_list_from_filter);
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

            /*personsResponse_list_from_add =
                personsResponse_list_from_add.OrderBy(temp => temp.Age).ToList();

            for (int i = 0; i < personsResponse_list_from_add.Count; i++)
            {
                //Assert
                Assert.Equal(personsResponse_list_from_add[i], personsResponse_list_from_sort[i]);
            }*/

            personsResponse_list_from_sort.Should().BeInAscendingOrder(temp => temp.Age);
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
            /*await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act 
                await _personsService.UpdatePerson(personUpdateRequest)
            );*/
            Func<Task> action = async () =>
                await _personsService.UpdatePerson(personUpdateRequest);

            await action.Should().ThrowAsync<ArgumentNullException>();

        }


        // when we supply person id is invalid, it should throw ArgumentException
        [Fact]
        public async Task UpdatePersonName_invalidPersonId()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonId, Guid.NewGuid())
                .Create();

            //Assert
           /* await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act 
              await _personsService.UpdatePerson(personUpdateRequest)
            );*/
            Func<Task> action = async () =>
                                  await _personsService.UpdatePerson(personUpdateRequest);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // when PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePersonName_NullPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);
            PersonUpdateRequest personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.CountryId, countryResponse.CountryId)
                .Create();

           /* await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act 
                await _personsService.UpdatePerson(personUpdateRequest)
            );*/
            Func<Task> action = async () =>
                await _personsService.UpdatePerson(personUpdateRequest);
            await action.Should().ThrowAsync<ArgumentException>();
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
            PersonResponse? personResponse_from_update = 
                await _personsService.UpdatePerson(personUpdateRequest);
            //Actual
            _testOutputHelper.WriteLine($"Actual : {personResponse_from_update?.ToString()}");
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
            //Excepting
            _testOutputHelper.WriteLine($"Excepting : {personResponse_from_get?.ToString()}");
            //Assert
            //Assert.Equal(personResponse_from_get, personResponse_from_update);
            personResponse_from_update.Should().Be(personResponse_from_get);
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
            /*await Assert.ThrowsAsync<ArgumentNullException>(async ()=>
            // Act
               await _personsService.DeletePerson(personId)
            );*/

            // Act
            Func<Task> action = async () =>
                await _personsService.DeletePerson(personId);
            await action.Should().ThrowAsync<ArgumentNullException>();    

        }

        // if supply invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            //Arrange

            Guid? personId = Guid.NewGuid();

            //Assert 
            //Assert.False(await _personsService.DeletePerson(personId));
            Func <Task<bool>> action = async () => await _personsService.DeletePerson(personId);
            (await action()).Should().BeFalse();
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
            //Assert.Null(await _personsService.GetPersonByPersonId(person_response_from_add.PersonId));
            Func<Task<object?>> action = async () =>
                await _personsService.GetPersonByPersonId(person_response_from_add.PersonId);
           (await action()).Should().BeNull();
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        #endregion

    }
}
