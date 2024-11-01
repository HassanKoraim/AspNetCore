using ServiceConstracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entities;
using System.Reflection;
using System.Xml.Linq;

namespace ServiceConstracts.DTO
{
    public class PersonResponse
    {
        [Required(ErrorMessage = "Person Id can't be blank")]
        public Guid? PersonId { get; set; }
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Given a Valid Email")]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Contry Id can't be blank")]
        public string? CountryId { get; set; }
        public bool? ReciveLetter { get; set; }

        public override string ToString()
        {
            return $"Perosn Id : {PersonId}, Person Name: {PersonName}, Email: {Email}\n" +
                $"Gender: {Gender}, CountryId: {CountryId},\n" +
                $"BirthDay: {BirthDate}, Address: {Address}, ReciveLetter : {ReciveLetter}";
        }
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse objConverted = obj as PersonResponse;
            return PersonId == objConverted.PersonId && PersonName == objConverted.PersonName &&
                Email == objConverted.Email && Gender == objConverted.Gender && BirthDate == objConverted.BirthDate 
                && CountryId == objConverted.CountryId
                && ReciveLetter == objConverted.ReciveLetter;
        }
    }
    
    public static class PersonExtension
    {
        public static PersonResponse? ToPersonResponse(this Person? person)
        {
            return new PersonResponse()
            {
                PersonId = person?.PersonId,
                PersonName = person?.PersonName,
                Email = person?.Email,
                CountryId = person.CountryId.ToString(),
               // Gender = (GenderOption)Enum.Parse(typeof(GenderOption),person?.Gender,true),
                Gender = person.Gender.ToString(),
                BirthDate = person.BirthDate,
                Address = person.Address,
                ReciveLetter = person.ReciveLetter
            };
        }
    }
}
