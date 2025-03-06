using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace RepositoryContracts
{
    /// <summary>
    /// Repersents data access logic for managing Person Entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Add a new Person object to the data store
        /// </summary>
        /// <param name="person">person to add</param>
        /// <returns>Returns a Person object after adding it to the data store</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all Persons in the data store
        /// </summary>
        /// <returns>List of Person object from table</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Returns a Person object based on the given Person Id
        /// </summary>
        /// <param name="PersonId">PersonId to search</param>
        /// <returns>Matching Person or null</returns>
        Task<Person> GetPersonByPersonId(Guid PersonId);

        /// <summary>
        /// Returns all Person objects based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ Expression to check</param>
        /// <returns>All Matching Persons with given condition </returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Delete Person object form data store
        /// </summary>
        /// <param name="PersonId">PersonId to search</param>
        /// <returns>Returns True, if Deletion is successfull;
        /// Otherwise, false</returns>
        Task<bool> DeletePersonByPersonId(Guid PersonId);

        /// <summary>
        /// Update Person object based on given Person Id
        /// </summary>
        /// <param name="person">Person object to Update</param>
        /// <returns>Returns a the Updated Person object </returns>
        Task<Person> UpdatePerson(Person person);
    }
}
