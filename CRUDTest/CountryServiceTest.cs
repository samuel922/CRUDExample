using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;

        public CountryServiceTest()
        {
            _countryService = new CountryService();
        }
        #region AddCountry
        //When the countryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountryAddrequest()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });
        }
        //When the CountryName is null, it should throw an ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        //When the countryName is duplicate - it should throw an ArgumentNullException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Japan" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Japan" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request2);
            });
        }

        //When proper country details are supplied - it should return a CountryResponse with new Guid ID
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "USA" };

            //Act
            CountryResponse response = _countryService.AddCountry(request);

            List<CountryResponse> actual_country_response_list = _countryService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty && response.CountryName != null);
            Assert.Contains(response, actual_country_response_list);
        }
        #endregion

        #region GetAllCountries
        //List of countries should be empty by default
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Arrange
            List<CountryResponse> country_response_list = _countryService.GetAllCountries();

            //Assert
            Assert.Empty(country_response_list);
        }

        //Add a few countries - the method should return the same objects that were added
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list =
            [
                new CountryAddRequest() { CountryName = "Japan" },
                new CountryAddRequest() { CountryName = "USA" },
                new CountryAddRequest() { CountryName = "Kenya" },
            ];

            List<CountryResponse> country_response_list_from_add = [];

            foreach(CountryAddRequest country_request in country_request_list)
            {
                country_response_list_from_add.Add(_countryService.AddCountry(country_request));
            }

            List<CountryResponse> actual_country_response_list = _countryService.GetAllCountries();

            //Assert
            foreach(CountryResponse expected_country_response in country_response_list_from_add)
            {
                Assert.Contains(expected_country_response, actual_country_response_list);
            }
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? CountryID = null;
            //Act
            CountryResponse? country_response_from_get = _countryService.GetCountryByCountryID(CountryID);

            //Assert
            Assert.Null(country_response_from_get);
        }

        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? country_add_request = new() { CountryName = "Japan" };

            CountryResponse? country_response_from_add = _countryService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = _countryService.GetCountryByCountryID(country_response_from_add.CountryID);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}