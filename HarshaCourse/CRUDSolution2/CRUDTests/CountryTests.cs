using ServiceConstracts;
using ServiceConstracts.DTO;
using Services;
using Xunit.Abstractions;
using Entities;

namespace CRUDTests
{
    public class CountryTests
    {
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public CountryTests(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddCountry

        // when we supply NullCountry, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            //Act
                _countriesService.AddCountry(countryAddRequest)
            );
        }
        // when we don't supply Country Name, it should throw ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = null
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
                 // Act
                 _countriesService.AddCountry(countryAddRequest)
            );
        }
        // when we supply duplicate Country Name, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            _countriesService.AddCountry(countryAddRequest1);
            //Assert
            Assert.Throws<ArgumentException>(()=>
                  // Act
                 _countriesService.AddCountry(countryAddRequest2)
            );
        }

        // if we supply Proper details, should be add Country into _countries list and Generate 
        // Country Id for it
        [Fact]
        public void AddCountry_ProperDetails()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };

            // Act 
            CountryResponse? countryResponse_from_add = _countriesService.AddCountry(countryAddRequest);
            _testOutputHelper.WriteLine(countryResponse_from_add.CountryId.ToString() );
            List<CountryResponse> countriesResponse_from_getAllCountries = _countriesService.GetAllCountries();
            //Assert
            Assert.True(countryResponse_from_add?.CountryId != Guid.Empty);
            Assert.Contains(countryResponse_from_add, countriesResponse_from_getAllCountries);
        }


        #endregion

        #region GetCountryByCountryId

        //Method to Create CountryResponse
        public CountryResponse? CreateCountry()
        {
            CountryAddRequest countryAdRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse? countryResponse = _countriesService.AddCountry(countryAdRequest);
            return countryResponse;
        }
        // if we supply null countryId, it should throw ArgumentNullException
        [Fact]
        public void GetCountryByCountryId_NullCountryId()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            //Act
            _countriesService.GetCountryByCountryId(null)
            );
        }

        // if we supply invalid Person Id, it should throw ArgumentException
        [Fact]
        public void GetCountryByCountryId_InvalidCountryId()
        {
            //Arrange
            Guid countryId = Guid.NewGuid();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
            _countriesService.GetCountryByCountryId(countryId)
            );
        }

        // if we supply Proper details, it should 
        [Fact]
        public void GetCountryByCountryId_ProperDetails()
        {
            //Arrange
             CountryResponse? countryResponse_from_create = CreateCountry();
            //Act
            CountryResponse? countryResponse_from_get =
                _countriesService.GetCountryByCountryId(countryResponse_from_create?.CountryId);
            _testOutputHelper.WriteLine(countryResponse_from_create?.CountryId.ToString());
            //Assert
            Assert.Equal(countryResponse_from_get, countryResponse_from_create);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_ReturnAllCountries()
        {
            //Arrange
            //Create Two Countries Response
            CountryResponse? countryResponse_from_create1 = CreateCountry();
            CountryAddRequest countryAdRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse? countryResponse_from_create2 = _countriesService.AddCountry(countryAdRequest);
            List<CountryResponse>? countriesResponse_from_create =
                new List<CountryResponse> { countryResponse_from_create1 , countryResponse_from_create2};
            // Print Expected List
           _testOutputHelper.WriteLine("Expected");
            foreach (CountryResponse expectedCountryResponse in countriesResponse_from_create)
            {
                _testOutputHelper.WriteLine(expectedCountryResponse.ToString()+"\n");
            }
            //Act 
            List<CountryResponse> countriesResponse_from_get = _countriesService.GetAllCountries();

            // Print Actual List
            _testOutputHelper.WriteLine("Actual");
            foreach (CountryResponse countryResponse in countriesResponse_from_get)
            {
                _testOutputHelper.WriteLine(countryResponse.ToString() + "\n");
            }

            //Assert
            Assert.Equal(countriesResponse_from_create, countriesResponse_from_get);
        }

        #endregion
    }
}