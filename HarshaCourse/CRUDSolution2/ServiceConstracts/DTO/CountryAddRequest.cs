using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceConstracts.DTO
{
    public class CountryAddRequest
    {
        [Required(ErrorMessage = "Country Name can't be blank")]
        public string CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = CountryName
            };
        }
    }
}