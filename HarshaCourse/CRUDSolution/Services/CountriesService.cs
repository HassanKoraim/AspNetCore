using Entities;
using ServiceConstracts;
using ServiceConstracts.DTO;
using System.Linq;
namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;
        public CountriesService(PersonsDbContext personsDbContext)
        {
            _db = personsDbContext;
        }
            public CountryResponse AddCountry(CountryAddRequest countryAddRequest)
        {
            //Validation: CountryAddRequest parameter can't be null
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));
            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            //Validation: CountryName can't be duplicate
            if(_db.Countries.Count(temp => temp.CountryName == 
            countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given Country Name is already exists");
            }
            
            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();
            //generate CountryId
            country.CountryId = Guid.NewGuid();
            // add country object into _countries
            _db.Countries.Add(country);
            _db.SaveChanges();
            return country.ToCountryResponse(); 
        }

        public List<CountryResponse> GetAllCountries()
        {
              return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country =  _db.Countries.FirstOrDefault(temp => temp.CountryId == countryId);
            if (country == null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }

    }
}
