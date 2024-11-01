using System.ComponentModel.DataAnnotations;
using System;

namespace Entities
{
    public class Person
    {
        [Required(ErrorMessage = "Person Id can't be blank")]
        public Guid PersonId { get; set; }
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Given a Valid Email")]
        public string? Email {  get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Contry Id can't be blank")]
        public Guid? CountryId { get; set; }
        public bool? ReciveLetter { get; set; }

    }
}
