using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
            public async Task<CountryResponse> AddCountry(CountryAddRequest countryAddRequest)
        {
            //Validation: CountryAddRequest parameter can't be null
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));
            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            //Validation: CountryName can't be duplicate
            if(await _db.Countries.CountAsync(temp => temp.CountryName == 
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
            await _db.SaveChangesAsync();
            return country.ToCountryResponse(); 
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
              return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country =  await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryId);
            if (country == null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
               ExcelWorksheet workSheet =
                    excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = workSheet.Dimension.Rows; // numbers of row 
                for(int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string countryName = cellValue;
                        if(_db.Countries.Where(temp => temp.CountryName == countryName).Count()== 0)
                        {
                            Country country = new Country() { CountryName = countryName };
                            _db.Countries.Add(country);
                            await _db.SaveChangesAsync();
                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}
