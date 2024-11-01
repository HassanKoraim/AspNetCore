using System;
using System.Collections.Generic;
using Entities;
using ServiceConstracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceConstracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid Email adrress")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        /// <summary>
        /// Converts the current PersonAddRequest object into a new Person object 
        /// </summary>
        /// <returns>Person Object</returns>
        public Person ToPerson()
        {
            //PersonId = Guid.NewGuid() ,
            return new Person { PersonName = PersonName, 
                Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), 
                CountryId = CountryId, Address = Address, ReceiveNewsLetter = ReceiveNewsLetter};

        }
    }
}
