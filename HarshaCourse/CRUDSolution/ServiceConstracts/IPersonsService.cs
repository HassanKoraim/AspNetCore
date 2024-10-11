using System;
using System.Collections.Generic;
using ServiceConstracts.DTO;

namespace ServiceConstracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new Person into the list of Persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Returns the same Person details, 
        /// along with newly generated PersonId</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        /// <summary>
        /// Returns all Persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();
        PersonResponse? UpdatePersonName(string? personName);
        void DeletePerson(string? personName);
        /// <summary>
        /// Returns Person object based on the given person id
        /// </summary>
        /// <param name="personId">Person Id to search</param>
        /// <returns>Returns matching person object</returns>
        PersonResponse? GetPersonByPersonId(Guid? personId);
    }
}
