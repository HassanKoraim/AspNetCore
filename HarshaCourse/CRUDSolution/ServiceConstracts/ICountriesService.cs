using ServiceConstracts.DTO;

namespace ServiceConstracts
{
    /// <summary>
    /// Repersents the business logic for minpulating 
    /// Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to the list of Countries 
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returns the Country object after adding it (including 
        /// newly generated Country id</returns>
        CountryResponse AddCountry(CountryAddRequest countryAddRequest);

        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as CountryResponse </CountryResponse></returns>
        List<CountryResponse> GetAllCountries();
        /// <summary>
        ///Returns a country object based on the given country id 
        /// </summary>
        /// <param name="countryId">CountryId (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        CountryResponse? GetCountryByCountryId(Guid? countryId);
    }
}
