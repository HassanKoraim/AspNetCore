using Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ServiceConstracts.DTO
{
    /// <summary>
    /// DTO Class that is used as return type for most of 
    /// CountriesService Method
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
        // It compares the current object to anthor object of CountryResponse type and
        // returns true, if both values are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;
            // type cast from object to CountryResponse
           // CountryResponse country_to_compare = obj as CountryResponse;  //Or
            CountryResponse country_to_compare = (CountryResponse)obj;
            return CountryId == country_to_compare.CountryId && 
                         CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse(){ CountryId = country.CountryId, 
                   CountryName = country.CountryName};
        }
    }
}
