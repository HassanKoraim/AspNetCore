using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Country
    {
        [Required(ErrorMessage = "Country Id can't be blank")]
        public Guid CountryId { get; set; }
        [Required(ErrorMessage = "Country Name can't be blank")]
        public string? CountryName { get; set; }
    }
}
