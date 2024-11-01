using Entities;
using ServiceConstracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace ServiceConstracts.DTO
{
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Represents the DTO class that contains th Person details to update 
        /// </summary>
        
        [Required (ErrorMessage = "Peson Id can't be blank")]
        public Guid PersonId { get; set; }
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
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };

        }
        public override string ToString()
        {
            return $"Person Id : {PersonId}, Person Name: {PersonName}," +
                $"Email: {Email}, Gender: {Gender}, " +
                $"Address: {Address},BirthDay: {DateOfBirth},  Country id : {CountryId}";
        }
    }
}
