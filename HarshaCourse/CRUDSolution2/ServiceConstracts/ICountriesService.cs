using ServiceConstracts.DTO;

namespace ServiceConstracts
{
    public interface ICountriesService
    {
        CountryResponse? AddCountry(CountryAddRequest? countryAddRequest);
        CountryResponse? GetCountryByCountryId(Guid? countryId);
        List<CountryResponse> GetAllCountries();
    }

}
