using System;
using System.Collections.Generic;
using Entities;
using ServiceConstracts.Enums;
namespace ServiceConstracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting new person
    /// </summary>
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        /// <summary>
        /// Converts the current PersonAddRequest object into a new Person object 
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person { PersonId = Guid.NewGuid() ,PersonName = PersonName, 
                Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), 
                CountryId = CountryId, Address = Address, ReceiveNewsLetter = ReceiveNewsLetter};

        }
    }
}
