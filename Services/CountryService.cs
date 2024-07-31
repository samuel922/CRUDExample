using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries;

        public CountryService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest)); 
            }
            //Validation: Null CountryName
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: Duplicate Country Name
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Any())
            {
                throw new ArgumentException("Given country name cannot be duplicate");
            }

            Country country = countryAddRequest.ToCountry();
            //Generate a new ID
            country.CountryID = Guid.NewGuid();

            //Add Country to list oc countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            Country? country_response_from_list =  _countries.FirstOrDefault(country => country.CountryID == countryID);

            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
