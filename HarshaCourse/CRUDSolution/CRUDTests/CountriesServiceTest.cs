using System;
using System.Collections.Generic;
using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
using Services;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        // Constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
           
        }

        #region AddCountry
        //when country is null, it should be throw argumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // arrange 
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            //Act
            await _countriesService.AddCountry(request)
            );
        }

        //when the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // arrange 
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null};

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act    
                await _countriesService.AddCountry(request) 
            );
        }

        //when the CountryName is duplicate, it should throw Argument Exception
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            // arrange 
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "Egypt" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "Egypt" };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            }
            );
        }

        //when you supply the proper CountryName, it should insert (Add) the Country to
        //the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // arrange 
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Egypt"};

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();
            //Assert 
            Assert.True( response.CountryId != Guid.Empty); 
            Assert.Contains(response, countries_from_GetAllCountries);
           
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of Countries should be empty by defualt (before
                          // adding any countries)
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();
            //Assert
            Assert.Empty(actual_country_response_list);

        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest { CountryName = "Egypt" },
                 new CountryAddRequest() { CountryName = "US" },
                 new CountryAddRequest() { CountryName = "Palastin" },
                 new CountryAddRequest() { CountryName = "Gazza" }
            };
            // Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach(var country in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country));
            }
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();
            //read each element from countries_list_from_add_country
            foreach(CountryResponse expected_country in countries_list_from_add_country)
            {
                //Assert
                Assert.Contains(expected_country, actualCountryResponseList);
            }
            
        }
        #endregion

        #region GetCountryByCountryId

        [Fact]
        // if we supply null as CountryId, it should return null as CountryResponse
        public async Task GetCountryByCountryId_GuidIsNull()
        {
            // arrange
            Guid? countryId = null;

            // act
            CountryResponse? country_response_from_get_country = await _countriesService.GetCountryByCountryId(countryId);

            //Assert 
            Assert.Null(country_response_from_get_country);
        }
        [Fact]
        // if we supply a valid CountryId, it should return the matching country details
        // as CountryResponse object
        public async Task GetCountryByCountryId_ValidCountryId()
        {

            // arrange 
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "Egypt" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            // Act 
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryId(country_response_from_add.CountryId);

            // Assert 
            Assert.Equal(country_response_from_add, country_response_from_get);
        }

        #endregion

    }
}
