using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person Entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new Country object to the data store
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Returns the Country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries in the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id;
        /// otherwise, it returns null
        /// </summary>
        /// <param name="CountryId">CountryId to search</param>
        /// <returns>Matching Country or null</returns>
        Task<char> GetCountryByCountryId(Guid CountryId);

        /// <summary>
        /// Returns a country object based on the given Country name
        /// </summary>
        /// <param name="CountryName">CountryName to search</param>
        /// <returns>Matching Country or null</returns>
        Task<Country> GetCountryByCountryName(string CountryName);
    }
}
