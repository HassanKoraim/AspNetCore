﻿using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {

        private readonly ApplicationDbContext _db;
        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            List<Country> countries = await _db.Countries.ToListAsync();
            return countries;
        }

        public async Task<Country> GetCountryByCountryId(Guid CountryId)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == CountryId);
        }

        public async Task<Country> GetCountryByCountryName(string CountryName)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == CountryName);

        }
    }
}
