using System;
using System.Collections.Generic;
using ServiceConstracts.DTO;
using ServiceConstracts.Enums;

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
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        /// <summary>
        /// Returns all Persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();
        /// <summary>
        /// Returns Person object based on the given person id
        /// </summary>
        /// <param name="personId">Person Id to search</param>
        /// <returns>Returns matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);
        /// <summary>
        /// Returns all persons object that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">search field to search</param>
        /// <param name="searchString">search string to search</param>
        /// <returns>Returns all matching Persons based on the given search
        /// field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);

        /// <summary>
        /// Returns Sorted List of Persons
        /// </summary>
        /// <param name="allPersons">Reperesent list of persons to sort</param>
        /// <param name="sortBy">Name of property (Key), based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse List</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified Person details based on the given Person Id 
        /// </summary>
        /// <param name="updatePersonRequest">Person details to update, including person id</param>
        /// <returns>Returns the person response object after updation</returns>
        Task<PersonResponse?> UpdatePerson(PersonUpdateRequest? updatePersonRequest);

        /// <summary>
        /// Deletes a Person based on given Person Id 
        /// </summary>
        /// <param name="personId">PersonId to delete</param>
        /// <returns>Returns true, if deletion is successful; otherwise false </returns>
        Task<bool> DeletePerson(Guid? personId);

        /// <summary>
        /// Returns Persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with CSV data of Persons</returns>
        Task<MemoryStream> GetPersonsCSV(List<PersonResponse> persons);

        /// <summary>
        /// Returns Persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of Persons</returns>
        Task<MemoryStream> GetPersonsExcel(List<PersonResponse> persons);

    }
}
