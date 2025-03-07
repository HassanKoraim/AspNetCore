using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonId(Guid PersonId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(person => person.PersonId == PersonId));
            int rowsDeleted = await _db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Countries").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Countries").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonId(Guid PersonId)
        {
            return await _db.Persons.Include("Countries").FirstOrDefaultAsync(temp => temp.PersonId == PersonId);
        }

        public async Task<Person?> UpdatePerson(Person person)
        {
            Person? matchingPerson = 
                await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == person.PersonId);
            if(matchingPerson == null)
                return person;
            matchingPerson.PersonId = person.PersonId;
            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.Address = person.Address;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Gender = person.Gender;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.ReceiveNewsLetter = person.ReceiveNewsLetter;
            matchingPerson.TIN = person.TIN;
            matchingPerson.Country = person.Country;
            await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
