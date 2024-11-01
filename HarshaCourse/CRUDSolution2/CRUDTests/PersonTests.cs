using ServiceConstracts;
using ServiceConstracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Entities;
using ServiceConstracts.Enums;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonTests
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonTests(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService();
            _personsService = new PersonsService(_countriesService);
            _testOutputHelper = testOutputHelper;
        }
        // Method to create a country
        private CountryResponse? createCountry()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse? countryResponse =
                _countriesService.AddCountry(countryAddRequest);
            return countryResponse;
        }
        // method to create Persons
        private List<PersonResponse> createPersons() 
        {
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse? countryResponse1 =
                _countriesService.AddCountry(countryAddRequest1);
            PersonAddRequest personAddRequest1 = new PersonAddRequest()
                {
                    PersonName = "Hassan",
                    Email = "Hassan@gmail.com",
                    Address = "Address For Hassan",
                    BirthDate = Convert.ToDateTime("2000-7-8"),
                    CountryId = countryResponse1.CountryId,
                    Gender = GenderOption.Male,
                    ReciveLetter = true
                };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "Gaza"
            };
            CountryResponse? countryResponse2 =
                _countriesService.AddCountry(countryAddRequest2);
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Hussien",
                Email = "Hussien@gmail.com",
                Address = "Address For Hussien",
                BirthDate = Convert.ToDateTime("2005-5-5"),
                CountryId = countryResponse2.CountryId,
                Gender = GenderOption.Male,
                ReciveLetter = false
            };
            CountryAddRequest? countryAddRequest3 = new CountryAddRequest()
            {
                CountryName = "Palastain"
            };
            CountryResponse? countryResponse3 =
                _countriesService.AddCountry(countryAddRequest3);
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Fatma",
                Email = "Fatma@gmail.com",
                Address = "Address For Fatma",
                BirthDate = Convert.ToDateTime("1990-5-8"),
                CountryId = countryResponse3.CountryId,
                Gender = GenderOption.Female,
                ReciveLetter = false
            };
            List<PersonResponse> personsList = new List<PersonResponse>()
            {
                _personsService.AddPerson(personAddRequest1),
                _personsService.AddPerson(personAddRequest2),
                _personsService.AddPerson(personAddRequest3)
            };
            return personsList;
        }

        #region AddPerson

        // when we supply NullPersonRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPersonRequest()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply Null Person Name, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            CountryResponse? countryResponse = createCountry();
            PersonAddRequest? personAddRequest = new PersonAddRequest() {
                PersonName = null, Address = "fjkas",
                BirthDate = Convert.ToDateTime("2000-7-8"), Email = "Example@gmail.com",
                CountryId = countryResponse?.CountryId,
                Gender = GenderOption.Male,
                ReciveLetter = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply Null Email, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullEmail()
        {
            //Arrange
            CountryResponse countryResponse = createCountry();
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Address = "Address for Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                Email = null,
                CountryId = countryResponse.CountryId,
                ReciveLetter = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply invalid Email, it should throw ArgumentException
        [Fact]
        public void AddPerson_InvalidEmail()
        {
            //Arrange
            CountryResponse countryResponse = createCountry();
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Address = "Address for Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                Email = "Hassan2gmail",
                CountryId = countryResponse.CountryId,
                ReciveLetter = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply Null Country Id, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullContryId()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Address = "Address for Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                Email = "Hassan@gmail.com",
                CountryId = null,
                ReciveLetter = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply invalid Country Id, it should throw ArgumentException
        [Fact]
        public void AddPerson_InvalidContryId()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Address = "Address for Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                Email = "Hassan@gmail.com",
                CountryId = Guid.NewGuid(),
                ReciveLetter = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
                _personsService.AddPerson(personAddRequest)
            );
        }

        // when we supply proper details
        [Fact]
        public void AddPerson_ValidPerson()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);
            _testOutputHelper.WriteLine($"This is { countryResponse.CountryName}");
            _testOutputHelper.WriteLine($"This is { countryResponse.CountryId.ToString()}");
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Address = "Address for Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                Email = "Hassan@gmail.com",
                CountryId = countryResponse?.CountryId,
                ReciveLetter = true
            };
            PersonResponse? personResponse = _personsService.AddPerson(personAddRequest);
            _testOutputHelper.WriteLine(personResponse.PersonId.ToString() );

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
        }

        #endregion

        #region GetPersonByPersonId

        // when we supply null Person Id, it should throw ArgumentNullException
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            Assert.Throws<ArgumentNullException>(() =>
            // Act 
            _personsService.GetPersonByPersonId(null));
        }

        // when we supply Invalid Person Id, it should throw ArgumentException
        [Fact]
        public void GetPersonByPersonId_InvalidPersonId()
        {
            //Arrange
            Guid? personId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() =>
            // Act 
            _personsService.GetPersonByPersonId(personId));
        }

        // when we supply proper Person Id , it should return the person
        [Fact]
        public void GetPersonByPersonId_ProperPersonId()
        {
            //Arrange
            List<PersonResponse> personsResponse_from_create = createPersons();

            // Act 
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personsResponse_from_create[0].PersonId);

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, personsResponse_from_create);  
        }

        #endregion Get

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_ReturnPersons()
        {
            //Arrange
            List<PersonResponse> personsResponse_from_create = createPersons();
            //Print Expected List
            _testOutputHelper.WriteLine("Expected");
            foreach(PersonResponse personResponse_from_create in personsResponse_from_create)
            {
                _testOutputHelper.WriteLine($"{personResponse_from_create} \n");
            }
            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetAllPersons();
            //Print Actual List
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse personResponse_from_get in personsResponse_from_get)
            {
                _testOutputHelper.WriteLine($"{personResponse_from_get} \n");
            }
            //Assert
            Assert.Equal(personsResponse_from_get, personsResponse_from_create);

        }
        #endregion

        #region GetFilterdPersons

        // if we supply null search by, it should throw ArgumentNullException
        [Fact]
        public void GetFilterdPersons_NullSearchBy()
        {
            //Arrange
            string? searchBy = null;
            string searchString = "Hassan";

            //Assert
            Assert.Throws<ArgumentNullException>(() => 
                //Act 
                _personsService.GetFilterdPersons(searchBy, searchString)
            );
        }

        // if we supply null search string, it should return the list without Filtering
        [Fact]
        public void GetFilterdPersons_NullSearchString()
        {
            //Arrange
            string searchBy = "PersonName";
            string? searchString = null;
            List<PersonResponse> personsResponse_from_create = createPersons();

            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetFilterdPersons(searchBy, searchString);

            //Assert
            Assert.Equal(personsResponse_from_create, personsResponse_from_get);
        }

        // if we pass PersonName to SearchBy Parameter, and Hassan to searchstring parameter,
        // it should return Filtred list that contains only Hassan in the PersonName
        [Fact]
        public void GetFilterdPersons_ProperDetails()
        {
            //Arrange
            string searchBy = "PersonName";
            string? searchString = "Hassan";
            List<PersonResponse> allPersons = createPersons();

            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetFilterdPersons(searchBy, searchString);

            //Assert
            List<PersonResponse>? personsResponse_expected = allPersons.Where(temp => temp.PersonName == searchString).ToList();

            Assert.Equal(personsResponse_expected, personsResponse_from_get);
        }


        #endregion
    }
}
