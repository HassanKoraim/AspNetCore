using System;
using System.Collections.Generic;
using Entities;

namespace ServiceConstracts.DTO
{
    public class CountryAddRequest
    {
        /// <summary>
        /// DTO Class for adding a new Country 
        /// </summary>
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country { CountryName = CountryName };
        }
    }
}
