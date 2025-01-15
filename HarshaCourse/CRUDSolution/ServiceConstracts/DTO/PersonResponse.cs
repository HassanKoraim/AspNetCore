using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceConstracts.Enums;

namespace ServiceConstracts.DTO
{
    /// <summary>
    /// Represents a DTO class that is used as return type of most method of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object 
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True or False, indicating whether all person details are mathched with 
        /// the specified parameter object </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse) ) return false;
            PersonResponse? personResponse = obj as PersonResponse;
            return PersonId == personResponse?.PersonId &&
                PersonName == personResponse.PersonName && Email == personResponse.Email && 
                DateOfBirth == personResponse.DateOfBirth && Gender == personResponse.Gender  &&
                CountryId == personResponse.CountryId && country == personResponse.country &&
                Address == personResponse.Address && ReceiveNewsLetter == personResponse.ReceiveNewsLetter;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person Id : {PersonId}, Person Name: {PersonName}," +
                $"Email: {Email}, Gender: {Gender}, Country: {country}" +
                $"Address: {Address},BirthDay: {DateOfBirth}, Age: {Age},  Country id : {CountryId}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions),Gender, true),
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter,
                CountryId = CountryId
            };

        }


    }
    public static class PersonExtensions
    {
        /// <summary>
        /// An Extension Method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">The Person object to convert</param>
        /// <returns>returns the converted PersonResponse object</returns>
        public static PersonResponse? ToPersonResponse(this Person? person)
        {
            return new PersonResponse() { PersonId = person.PersonId,
                PersonName = person.PersonName, Email = person.Email,
                DateOfBirth = person.DateOfBirth, Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address, ReceiveNewsLetter = person.ReceiveNewsLetter,
                Age = (person.DateOfBirth != null)? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays/365.25): null,
                country = person.Country?.CountryName};
        }
    }
}
