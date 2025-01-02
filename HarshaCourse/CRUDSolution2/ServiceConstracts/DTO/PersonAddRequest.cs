using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;
using Entities;
using ServiceConstracts.Enums;

namespace ServiceConstracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Given a Valid Email")]
        public string? Email { get; set; }
        public GenderOption? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Contry Id can't be blank")]
        public Guid? CountryId { get; set; }
        public bool? ReciveLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                Gender = Gender.ToString(),
                DateOfBirth = BirthDate,
                Address = Address,
                CountryId = CountryId,
                ReceiveNewsLetter = ReciveLetter
            };
        }
    }
}
