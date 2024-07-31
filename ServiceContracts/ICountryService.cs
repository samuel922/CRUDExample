using ServiceContracts.DTO;

namespace ServiceContracts
{
    ///<summary>
    ///Represents the business logic for manipulating the country entity
    /// </summary>
    /// 
    public interface ICountryService
    {
        /// <summary>
        /// Add a Country
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Get a list of all countries on the list
        /// </summary>
        /// <returns>List of CountryResponse objects</returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns CountryResponse object from a list CountryResponse objects that matches the given ID
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Country =Response object with the matching CountryID</returns>
        CountryResponse? GetCountryByCountryID(Guid? countryID);
    }
}
