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

        #region GetSortedPersons
        // if we supply null List, it should throw ArrgumentNullException
        [Fact]
        public void GetSortedPersons_nullList()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                   //Act
                 _personsService.GetSortedPersons(null, "PersonName", sortOrderOption.ASC)
            );
        }

        // if supply null value in SortyBy, it should return all List without arrangement
        [Fact]
        public void GetSortedPersons_NullSortBy()
        {
            // Arrange
            List<PersonResponse>? personsResponse_from_Create = createPersons();

            // act 
            List<PersonResponse>? personsResponse_from_get = _personsService.GetSortedPersons(personsResponse_from_Create,null, sortOrderOption.ASC);
            // Assert 
            Assert.Equal(personsResponse_from_Create, personsResponse_from_get);
        }

        // if we supply null sortOrderOption, it should return list without arrangment
        [Fact]
        public void GetSortedPerson_nullsortOrderOption()
        {
            // Arrange
            List<PersonResponse>? personsResponse_from_Create = createPersons();

            // act 
         //   List<PersonResponse>? personsResponse_OrderByPersonNamewithAsc = personsResponse_from_Create.OrderBy(person => person.PersonName).ToList();
            List<PersonResponse>? personsResponse_from_get = _personsService.GetSortedPersons(personsResponse_from_Create, "PersonName", null);

            // Assert 
            Assert.Equal(personsResponse_from_Create, personsResponse_from_get);
        }
        [Fact]
        public void GetSortedPerson_withPersonName_inAscOrder()
        {
            // arrange
            List<PersonResponse> personsResponse_from_create = createPersons();

            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetSortedPersons(personsResponse_from_create, "PersonName", sortOrderOption.ASC);
            List<PersonResponse>? personsResponse_Sorted = personsResponse_from_create.OrderBy(x => x.PersonName).ToList();

            //Expected
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse personResponse in personsResponse_from_get)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            _testOutputHelper.WriteLine("Sorted List");
            foreach(PersonResponse personResponse in personsResponse_Sorted)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            //Assert
            Assert.Equal(personsResponse_Sorted, personsResponse_from_get);
        }

        [Fact]
        public void GetSortedPerson_withPersonName_inDescOrder()
        {
            // arrange
            List<PersonResponse> personsResponse_from_create = createPersons();

            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetSortedPersons(personsResponse_from_create, "PersonName", sortOrderOption.DESC);
            List<PersonResponse>? personsResponse_Sorted = personsResponse_from_create.OrderByDescending(x => x.PersonName).ToList();

            //Expected
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse personResponse in personsResponse_from_get)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            _testOutputHelper.WriteLine("Sorted List");
            foreach (PersonResponse personResponse in personsResponse_Sorted)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            //Assert
            Assert.Equal(personsResponse_Sorted, personsResponse_from_get);
        }

        [Fact]
        public void GetSortedPerson_withCountryName_inDescOrder()
        {
            // arrange
            List<PersonResponse> personsResponse_from_create = createPersons();

            //Act
            List<PersonResponse>? personsResponse_from_get = _personsService.GetSortedPersons(personsResponse_from_create, "CountryName", sortOrderOption.DESC);
            List<PersonResponse>? personsResponse_Sorted = personsResponse_from_create.OrderByDescending(x => x.CountryName).ToList();

            //Expected
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse personResponse in personsResponse_from_get)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            _testOutputHelper.WriteLine("Sorted List");
            foreach (PersonResponse personResponse in personsResponse_Sorted)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }
            //Assert
            Assert.Equal(personsResponse_Sorted, personsResponse_from_get);
        }
        #endregion

        #region DeletePerson

        // when we supply null Guid, it should throw ArgumentNullException
        [Fact]
        public void DeletePerson_withNullGuid()
        {
            //Arrange
            Guid guid = Guid.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() => 
                //Act 
                _personsService.DeletePerson(guid)
            );

        }

        // when we supply Guid doesn't Exist in the Persons,
        // it should throw ArgumentException with wrong message
        [Fact]
        public void DeletePerson_withGuidDoesntExist()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            
            Assert.Throws<ArgumentException>(()=>
                //Act 
                _personsService.DeletePerson(guid)
            );
        }

        // when we supply the proper details, it should delete the person from the _persons
        [Fact]
        public void DeletePerson_withProperDetails()
        {
            //Arrange
            List<PersonResponse> personsReponse = createPersons();
            Guid? guid = personsReponse.ElementAt(0).PersonId;

            //Assert
            Assert.True(
                //Act
                _personsService.DeletePerson(guid)
                );
            Assert.Throws<ArgumentException>(() => _personsService.GetPersonByPersonId(guid));
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_withNullpersonRequest()
        {
            //Arrange 
            PersonUpdateRequest? personRequest = null;

            // Assert
             Assert.Throws<ArgumentNullException>(()=> _personsService.UpdatePerson(personRequest));

        }
        [Fact]
        public void UpdatePerson_withDoesNotExsitPerson()
        {
            //Arrange 
            PersonUpdateRequest? personRequest = new PersonUpdateRequest()
            {
                PersonName = "AbdAllah",
                PersonId = Guid.NewGuid(),
                Address = "Elwakf",
                Email = "Hasdfh@gmail.com",
                CountryId = Guid.NewGuid()
            };

            //act

            // Assert
            Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(personRequest));

        }
        [Fact]
        public void UpdatePerson_withProperDetails()
        {
            //Arrange 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse? countryResponse =
                _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Hassan",
                Email = "Hassan@gmail.com",
                Address = "Address For Hassan",
                BirthDate = Convert.ToDateTime("2000-7-8"),
                CountryId = countryResponse.CountryId,
                Gender = GenderOption.Male,
                ReciveLetter = true
            };
            PersonResponse? personResponse = _personsService.AddPerson(personAddRequest);
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest()
            {
                PersonName = "AbdAllah",
                PersonId = personResponse?.PersonId,
                Address = "Elwakf",
                Email = "Hasdfh@gmail.com",
                CountryId = personAddRequest.CountryId
            };

            //Act 
            PersonResponse? personResponse_from_update = _personsService.UpdatePerson(personUpdateRequest);
            PersonResponse? personResponse_from_list = _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            // Assert
            Assert.Equal(personResponse_from_list, personResponse_from_update);

        }

            #endregion


        }
}
