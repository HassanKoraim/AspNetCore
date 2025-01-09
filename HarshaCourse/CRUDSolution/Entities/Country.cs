using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        /// <summary>
        /// Domain Model for Country
        /// </summary>
        [Key]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
