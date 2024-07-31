using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Business logic for manipulating Person Entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Add Person to a list of Persons object
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns>Return a PersonResponse object</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Get all Persons
        /// </summary>
        /// <returns>List of all PersonResponse objects</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Gets the matching person with the given personID from a list of Person's objects
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns>PersonResponse with the given personID</returns>
        PersonResponse? GetPersonByPersonID(Guid? PersonID);

        /// <summary>
        /// Retuns all personResponse objects with the matching sorting creteria
        /// </summary>
        /// <param name="searchBy">Property to search by</param>
        /// <param name="searchString">Piece of text to search</param>
        /// <returns>PersonResponse objects</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
        /// <summary>
        /// Returns a List of PersonResponse in a given sort order.
        /// </summary>
        /// <param name="allPersons">List of all persons</param>
        /// <param name="sortBy">Property name to sort by</param>
        /// <param name="sortOrder">ASC || DESC</param>
        /// <returns>PersonResponse objects</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the give Person details
        /// </summary>
        /// <param name="personUpdateRequest">Person object to update</param>
        /// <returns>Updated object</returns>
        PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);

       bool DeletePerson(Guid? personID);
    }
}
