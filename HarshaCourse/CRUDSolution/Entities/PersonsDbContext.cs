﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // seed to Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries =
                System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach (var country in countries)
                modelBuilder.Entity<Country>().HasData(country);

            // Seed to Persons
            string personsJson = System.IO.File.ReadAllText("persons.Json");
            List<Person> persons =
                System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (var person in persons)
                modelBuilder.Entity<Person>().HasData(person);

        }
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetter", person.ReceiveNewsLetter)
            };
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId, @PersonName," +
                " @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetter", parameters);
        }
    }
}