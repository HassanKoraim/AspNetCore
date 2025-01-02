using ServiceConstracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entities;
using System.Reflection;
using System.Xml.Linq;
using System.Diagnostics.CodeAnalysis;

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
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Contry Id can't be blank")]
        public string? CountryId { get; set; }
        public bool? ReceiveNewsLetter { get; set; }
        public string? CountryName { get; set; }
        public double? Age { get; set; }
        public override string ToString()
        {
            return $"Perosn Id : {PersonId}, Person Name: {PersonName}, Email: {Email}\n" +
                $"Gender: {Gender}, CountryId: {CountryId},\n" + $"Country Name: {CountryName} "+
                $"BirthDay: {DateOfBirth}, Address: {Address}, ReciveLetter : {ReceiveNewsLetter}";
        }
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse objConverted = obj as PersonResponse;
            return PersonId == objConverted.PersonId && PersonName == objConverted.PersonName &&
                Email == objConverted.Email && Gender == objConverted.Gender && DateOfBirth == objConverted.DateOfBirth 
                && CountryId == objConverted.CountryId
                && ReceiveNewsLetter == objConverted.ReceiveNewsLetter;
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
                DateOfBirth = person.DateOfBirth,
                Address = person.Address,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            
            };
        }
    }
}
