using System;
using System.Collections.Generic;
using Entities;

namespace ServiceConstracts.DTO
{
    public class CountryResponse
    {
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse? countryResponse = obj as CountryResponse;
            return CountryId == countryResponse?.CountryId && CountryName == countryResponse?.CountryName;
        }
        public override string ToString()
        {
            return $"Country Id : {CountryId}, CountryName : {CountryName}";
        }
    }

    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    } 
}
