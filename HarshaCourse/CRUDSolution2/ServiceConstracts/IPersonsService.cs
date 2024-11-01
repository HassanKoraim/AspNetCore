using ServiceConstracts.DTO;
using System;
using System.Collections.Generic;
using ServiceConstracts.Enums;

namespace ServiceConstracts
{
    public interface IPersonsService
    {
        PersonResponse? AddPerson(PersonAddRequest? personAddRequest);
        PersonResponse? GetPersonByPersonId(Guid? personId);
        List<PersonResponse>? GetAllPersons();
        List<PersonResponse> GetFilterdPersons(string? searchBy, string? searchString);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons , string? sortBy, sortOrderOption? sortOrder);
        PersonResponse UpdatePerson(Guid? personId);
        bool DeletePerson(Guid? personId);
    }
}
