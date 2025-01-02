using System.Diagnostics.Tracing;
using ServiceConstracts;
using ServiceConstracts.DTO;
using Entities;
using System.ComponentModel.DataAnnotations;
using Services.Helper;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country>? _countries;
        public CountriesService(bool init = true)
        {
            _countries = new List<Country>();
            if (init)
            {
                _countries.AddRange(new List<Country>() {
                    new Country(){CountryId = Guid.Parse("C01A78A7-98CE-41C1-8D52-B4279120F9EF"), CountryName = "Gaza" },
                    new Country(){CountryId = Guid.Parse("8E9D4111-1C27-41E0-AAFB-360A11887FBC"), CountryName = "Egypt" },
                    new Country(){CountryId = Guid.Parse("A81AD7BA-96B6-40F8-817B-1FB7AA02B89F"), CountryName = "Palastine" },
                    new Country(){CountryId = Guid.Parse("A4E6C7A2-8306-4116-B9FA-3791CC2B0529"), CountryName = "Libia" },
                    new Country(){CountryId = Guid.Parse("7099A3F2-9820-4EC0-BCF5-ECB968BD516F"), CountryName = "Emarat" }
                });
            }
        }
       public CountryResponse? AddCountry(CountryAddRequest? countryAddRequest)
            {
                // check if countryAddRequest is not null
                if(countryAddRequest == null) throw new ArgumentNullException(nameof(countryAddRequest));
                //check if Country Name is exists 
                ValidationHelper.ModelValidation(countryAddRequest);
                //check if the CountryName is duplicate or not
                Country? country_from_countries = _countries?.FirstOrDefault(temp => temp.CountryName == countryAddRequest?.CountryName);
                if (country_from_countries != null) throw new ArgumentException(nameof(countryAddRequest));

                // Add Country
                // Convert CountryAddRequest object to Country Object
                Country country = countryAddRequest.ToCountry();
                // Generate Country Id
                country.CountryId = Guid.NewGuid();
                // save it in _countries List
                _countries?.Add(country);
                return country.ToCountryResponse();
            }
        /*
        public CountryResponse AddCountry(CountryAddRequest countryAddRequest)
        {
            //Validation: CountryAddRequest parameter can't be null
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));
            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            //Validation: CountryName can't be duplicate
            if (_countries.Where(temp => temp.CountryName ==
            countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given Country Name is already exists");
            }

            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();
            //generate CountryId
            country.CountryId = Guid.NewGuid();
            // add country object into _countries
            _countries.Add(country);
            return country.ToCountryResponse();
        }
        */
        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if( countryId == null) throw new ArgumentNullException( nameof(countryId));
            Country? country = _countries?.FirstOrDefault(temp => temp.CountryId == countryId);
            if(country == null) throw new ArgumentException("Given Valid Country Id");
            return country.ToCountryResponse(); 
        }
        /*
        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country = _countries.FirstOrDefault(temp => temp.CountryId == countryId);
            if (country == null)
            {
                throw new ArgumentException("Given a Valid Country Id");
            }
            return country.ToCountryResponse();
        }
        */

        public List<CountryResponse>? GetAllCountries()
        {
            List<CountryResponse>? countriesResponse = _countries?.Select(temp => temp.ToCountryResponse()).ToList();
            return countriesResponse;
        }


    }
}
