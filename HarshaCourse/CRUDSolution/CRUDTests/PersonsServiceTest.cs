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
using RepositoryConstracts;
using AutoFixture.Kernel;
using System.Linq.Expressions;
using CsvHelper;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        //Constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            /*
             * This is without DbContextMock
             * var CountriesIntialData = new List<Country>();
            var personsIntailData = new List<Person>();
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object ;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, CountriesIntialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons,personsIntailData);
            _countriesService = new CountriesService(dbContext);
            _personsService = new PersonsService(dbContext,_countriesService);*/

            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;
            var CountriesIntialData = new List<Country>();
            var personsIntailData = new List<Person>();
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, CountriesIntialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsIntailData);
            _countriesService = new CountriesService(null);
            _personsService = new PersonsService(_personsRepository);
            _testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        // When supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
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
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();
            Person person = personAddRequest.ToPerson();
            _personsRepositoryMock.
                Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
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
            Person person1 = personAddRequest1.ToPerson();
            Person person2 = personAddRequest2.ToPerson();
            // when call GetAllPersons, returns empty list, for first insertion
            _personsRepositoryMock
                .Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(new List<Person>());
            _personsRepositoryMock
                .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person1)
                .Callback<Person>( p =>
                {
                    _personsRepositoryMock
                    .Setup(temp => temp.GetAllPersons())
                    .ReturnsAsync(new List<Person>() {p});

                });
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
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            //Arrange 
            PersonAddRequest? personAddRequest =
                _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@Example.com")
                .Create();
            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();
            _personsRepositoryMock
                .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // to set the GetAllPerson with empty list
            _personsRepositoryMock
                .Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(new List<Person>());
                /*_fixture.Create<PersonAddRequest>();*/
                /*new PersonAddRequest {
                PersonName = "Hassan 52548", Email = "HassanKoraim2@gmail.com", Address = "Elwakf",
                CountryId = Guid.Parse("8F30BEDC-47DD-4286-8950-73D8A68E5D41"), Gender = GenderOptions.Male, ReceiveNewsLetter = true };*/
        //Act
        PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            person_response_expected.PersonId = person_response_from_add.PersonId;
            //Assert
            //Assert.True(personResponse.PersonId != Guid.Empty);
            person_response_from_add.PersonId.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);

        }
        #endregion

        #region GetPersonByPersonId

        [Fact]
        public void GetPersonByPersonId_NullPersonId_ToBeNull()
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
        public async Task GetPersonByPersonId_PersonIsNull_toBeNull()
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
        public async Task GetPersonByPersonId_ProperPerson_ToBeSuccessful()
        {
            //Arrange 
   /*         CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse =
                await _countriesService.AddCountry(countryRequest);*/

            Person person = 
                _fixture.Build<Person>()
                .With(temp => temp.Email, "HassanKoriam2@gmail.com")
                .Without(temp => temp.Country)
                .Create();
           /* Person person = personRequest.ToPerson();
            person.PersonId = Guid.NewGuid();*/
            PersonResponse person_response_expected = person.ToPersonResponse();
            // PersonResponse? personResponse_from_add = await _personsService.AddPerson(personRequest);
            _personsRepositoryMock
                 .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                 .ReturnsAsync(person);
                
            //Act
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(person_response_expected.PersonId);

            //Assert
            //Assert.Equal(personResponse_from_add, personResponse_from_get);
            personResponse_from_get.Should().Be(person_response_expected);
        }
        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPersons_EmptyList_ToBeEmptyList()
        {
            //Arrange
            //   List<Person>? persons = null;

            _personsRepositoryMock
                .Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(new List<Person>());
            //Act
            List<PersonResponse> person_response_from_get = await _personsService.GetAllPersons();

            //Assert.Empty(person_response_from_get);
            person_response_from_get.Should().BeEmpty();

        }
        // First, We 

        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
/*            List<PersonResponse> personsResponse_list_from_add = await createListOfPersonsResponse();
            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }*/
            List<Person> persons_list = new List<Person>();
            for(int i = 0; i < 3; i++)
            {
                persons_list.Add(
                _fixture.Build<Person>()
                .Without(temp => temp.Country)
                .Create());
            };
            List<PersonResponse> persons_response_expected = persons_list.Select(person => person.ToPersonResponse()).ToList();
            _personsRepositoryMock
                .Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons_list);
            //Act
            List<PersonResponse> personsResponse_list_from_get = await _personsService.GetAllPersons();

            //print personsResponse_list_from_get
            /*_testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_get)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }*/

            //Assert
            /*foreach (PersonResponse personResponse_from_add in personsResponse_list_from_add)
            {
                //Assert
                Assert.Contains(personResponse_from_add, personsResponse_list_from_get);
            }*/
            personsResponse_list_from_get.Should().BeEquivalentTo(persons_response_expected);
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
        public async Task GetFilteredPersons_EmptySearchText_ToBeAllPersons()
        {
            List<Person> persons = new List<Person>() {

                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "HassanKoraim")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Hussian")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Fatma")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
            };
            
            List<PersonResponse> persons_response_expected = 
                persons.Select(temp => temp.ToPersonResponse()).ToList();
            
            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_expected in persons_response_expected)
            {
                _testOutputHelper.WriteLine(person_response_expected.ToString());
            }

            _personsRepositoryMock.
                Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            //Act
            List<PersonResponse> personsResponse_list_from_search =
                await _personsService.GetFilteredPersons(nameof(Person.PersonName),"");

            //print personsResponse_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personsResponse_list_from_search)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            personsResponse_list_from_search.Should().BeEquivalentTo(persons_response_expected);
        }
        //First we will add few Persons; and then we will search based on person name 
        // with some search string, it should be return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSpecificMatchedPersons()
        {
            //Arrange
            List<Person> persons = new List<Person>() {

                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "HassanKoraim")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Hussian")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Fatma")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
            };
            List<Person> persons_expected = persons.Where(temp => temp.PersonName.Contains("Ha",StringComparison.OrdinalIgnoreCase)).ToList();
            List<PersonResponse> persons_response_expected = persons_expected.Select(temp => temp.ToPersonResponse()).ToList();

            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_expected in persons_response_expected)
            {
                _testOutputHelper.WriteLine(person_response_expected.ToString());
            }

            _personsRepositoryMock.
                 Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                 .ReturnsAsync(persons_expected);
            //Act
            List<PersonResponse> personsResponse_list_from_search =
                await _personsService.GetFilteredPersons(nameof(Person.PersonName), "Ha");

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
           personsResponse_list_from_search.Should().BeEquivalentTo(persons_response_expected);
            //personsResponse_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("Ha", StringComparison.OrdinalIgnoreCase));
        }

        // That's I make it 
        [Fact]
        public async Task GetFilteredPersons_NullPara()
        {
            //Arrange
            string? SearchBy = null;
            string? SearchString = null;
            List<Person> persons = new List<Person>() {

                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "HassanKoraim")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Hussian")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Fatma")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
            };
            List<PersonResponse> personsResponse_list = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //Assert
            /*await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    //Act
                     await _personsService.GetFilteredPersons(SearchBy, SearchString)
            );*/
            _personsRepositoryMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);
            List<PersonResponse> personsResponse_list_from_filter =
                 await _personsService.GetFilteredPersons(SearchBy, SearchString);
            personsResponse_list.Should().BeEquivalentTo(personsResponse_list_from_filter);
        }

        #endregion

        #region GetSortedPersons

        //when we sort based on PersonName in DESC, it should return Persons list in descending 
        // on PersonName 
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {

                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "HassanKoraim")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Hussian")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
                    _fixture.Build<Person>()
                    .With(temp => temp.PersonName, "Fatma")
                    .With(temp => temp.Email, "HassanKoraim2@gmail.com")
                    .With(temp => temp.Country, null as Country)
                    .Create(),
            };
            List<PersonResponse> personsResponse = 
                 persons.Select(temp => temp.ToPersonResponse()).ToList();
            List<PersonResponse> persons_response_expected =
                personsResponse.OrderBy(temp => temp.PersonName).ToList();
            //print personsResponse_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in persons_response_expected)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = personsResponse;
            List<PersonResponse> personsResponse_list_from_sort =
                await _personsService.GetSortedPersons(allPersons,
                nameof(PersonResponse.PersonName), SortOrderOptions.ASC);

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

            personsResponse_list_from_sort.Should().BeInAscendingOrder(temp => temp.PersonName);
            personsResponse_list_from_sort.Should().BeEquivalentTo(persons_response_expected);
        }

        #endregion

        #region UpdatePersonName


        // when PersonUpdateRequest is null,it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePersonName_NullPersonUpdateRequest_ToBeArgumentNullException()
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
        public async Task UpdatePersonName_invalidPersonId_ToBeArgumentException()
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
            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(null as  Person);

            Func<Task> action = async () =>
                                  await _personsService.UpdatePerson(personUpdateRequest);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // when PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePersonName_NullPersonName_ToBeArgumentException()
        {
            //Arrange
          /*  CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);*/
            PersonUpdateRequest personUpdateRequest = 
                _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.CountryId, (Guid?) null)
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
        public async Task UpdatePersonName_PersonFullDetailsUpdation_ToBeSuccessful()
        {
            //Arrange

            Person person =
                _fixture.Build<Person>()
                .With(temp => temp.Email, "Hassan@gmail.com")
                .Without(temp => temp.Country)
                .With(temp => temp.Gender, "Male")
                //.With(temp => temp.Country, new Country() { CountryId = Guid.Parse("9BA5BEDB-843E-4A7A-BC70-C6385807393C"), CountryName ="Egypt"})
                .Create();
            PersonResponse personResponse_expected= person.ToPersonResponse();
            PersonUpdateRequest personUpdateRequest = personResponse_expected.ToPersonUpdateRequest();
/*            // Change the PersonName and Email
            personUpdateRequest.PersonName = "Ibrahim";
            personUpdateRequest.Email = "Ibrahim@gmail.com";
*/
            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _personsRepositoryMock
                .Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            //Act
            PersonResponse? personResponse_from_update = 
                await _personsService.UpdatePerson(personUpdateRequest);
           /* //Actual
            _testOutputHelper.WriteLine($"Actual : {personResponse_from_update?.ToString()}");
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
            //Excepting
            _testOutputHelper.WriteLine($"Excepting : {personResponse_from_get?.ToString()}");*/
            //Assert
            //Assert.Equal(personResponse_from_get, personResponse_from_update);
            personResponse_from_update.Should().Be(personResponse_expected);
        }


        #endregion

        #region DeletePerson

        // if Supply PersonDeleteRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task DeletePerson_personIdIsNull_ToBeArgumentNullException()
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
        public async Task DeletePerson_InvalidPersonId_ToBeFalse()
        {
            //Arrange

            Guid? personId = Guid.NewGuid();
            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);
            //Assert 
            //Assert.False(await _personsService.DeletePerson(personId));
            Func <Task<bool>> action = async () => await _personsService.DeletePerson(personId);
            (await action()).Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ValidPerson_ToBeSuccessful() 
        {
            //Arragne
            Person person = 
                _fixture.Build<Person>()
                .With(temp => temp.Email, "Hassan@gmail.com")
                .Without(temp => temp.Country)
                .Create();
            PersonResponse personRespose = person.ToPersonResponse();
            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _personsRepositoryMock
                .Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            // Act 
            bool isDeleted = await _personsService.DeletePerson(person.PersonId);

            // Assert
            //Assert.Null(await _personsService.GetPersonByPersonId(person_response_from_add.PersonId));
/*            Func<Task<object?>> action = async () =>
                await _personsService.GetPersonByPersonId(personRespose.PersonId);
            (await action()).Should().BeNull();*/
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        #endregion

    }
}
