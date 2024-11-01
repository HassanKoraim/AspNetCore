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
        public CountriesService()
        {
            _countries = new List<Country>();
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
