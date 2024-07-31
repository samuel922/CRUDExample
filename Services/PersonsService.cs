using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountryService _countryService;
        public PersonsService()
        {
            _persons = new List<Person>();
            _countryService = new CountryService();
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest)); 
            }

            //Model Validations
            ValidationHelper.ModelValidation(personAddRequest);

            //Convert PersonAddRequest to Person Object
            Person person = personAddRequest.ToPerson();

            //Generate a new PersonId
            person.PersonID = Guid.NewGuid();

            //Add person to list of persons
            _persons.Add(person);   

            return ConvertPersonToPersonResponse(person);
            
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? PersonID)
        {
            if(PersonID == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(person => person.PersonID == PersonID);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            switch(searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(person =>
                    (!string.IsNullOrEmpty(person.PersonName) ? person.PersonName.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(person => 
                    (!string.IsNullOrEmpty(person.Email)) ? person.Email.Contains(searchString, 
                    StringComparison.OrdinalIgnoreCase) : true).ToList(); 
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(person =>
                    person.DateOfBirth != null ? person.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, 
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(person =>
                    (!string.IsNullOrEmpty(person.Address)) ? person.Address.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(person =>
                    (!string.IsNullOrEmpty(person.Gender)) ? person.Gender.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Country):
                    matchingPersons = allPersons.Where(person =>
                    (!string.IsNullOrEmpty(person.Country)) ? person.Country.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default:
                    return matchingPersons;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (sortBy == null)
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase)],
                
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.DateOfBirth)],

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.DateOfBirth)],

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.Age)],

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.Age)],

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.Address, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.Address, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase)],

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC) =>
                [.. allPersons.OrderBy(person => person.ReceiveNewsLetter)],

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC) =>
                [.. allPersons.OrderByDescending(person => person.ReceiveNewsLetter)],
                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(personUpdateRequest);

            //Model Validations
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Retrive the Person object with the given ID
            Person? person = _persons.FirstOrDefault(person => person.PersonID == personUpdateRequest.PersonID);

            if (person == null)
            {
                throw new ArgumentException("PersonID cannot be null or empty");
            }

            //Update all the fields
            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.Address = personUpdateRequest.Address;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;
            person.CountryID = personUpdateRequest.CountryID;

            return person.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            ArgumentNullException.ThrowIfNull(nameof(personID));

            Person? person = _persons.FirstOrDefault(person => person.PersonID == personID);

            if (person == null)
            {
                return false;
            }
            _persons.RemoveAll(person => person.PersonID == personID);

            return true;
        }
    }
}
