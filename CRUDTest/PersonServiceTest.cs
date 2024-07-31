using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Entities;
using Xunit.Abstractions;

namespace CRUDTest
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _countryService = new CountryService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        //When PersonAddRequest is null - ArgumentNullException is thrown
        [Fact]
        public void AddPerson_NullPersonID()
        {
            //Arrange
            PersonAddRequest? person_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(person_request);
            });
            
        }

        //When a null PersonName is supplied - throws Argument Exception
        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? person_request = new PersonAddRequest()
            {
                PersonName = null,
                Email = "onyango@gmail.com",
                DateOfBirth = Convert.ToDateTime("2000-03-05"),
                Address = "Nairobi, Kenya",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(person_request);
            });
        }

        //When the correct person details is supplied - new PersonResponse with a new PersonID is generated
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            CountryAddRequest? country_request = new() { CountryName = "Kenya"};
            CountryResponse country_response = _countryService.AddCountry(country_request);
            PersonAddRequest? person_request = new PersonAddRequest()
            {
                PersonName = "Samuel",
                Email = "onyango@gmail.com",
                DateOfBirth = Convert.ToDateTime("2000-03-05"),
                CountryID = country_response.CountryID,
                Address = "Nairobi, Kenya",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
            };

            //Act
            PersonResponse person_response = _personsService.AddPerson(person_request);
            List<PersonResponse> person_response_from_get = _personsService.GetAllPersons();

            //Assert
            Assert.True(person_response.PersonID != Guid.Empty);
            Assert.Contains(person_response, person_response_from_get);
        }
        #endregion

        #region GetPersonByPersonID
        //When the PersonID is null - throw ArgumentNullException
        [Fact]
        public void GetPersonByPerosnID_NullPersonID()
        {
            //Arrange
            Guid? PersonID = null;

            //Act
            PersonResponse? person_response = _personsService.GetPersonByPersonID(PersonID);

            //Assert
            Assert.Null(person_response);
        }

        [Fact]
        public void GetPersonByPersonID_ValidPersonID()
        {
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Kenya" };
            CountryResponse country_response_from_add = _countryService.AddCountry(country_request);

            //Arrange
            PersonAddRequest? person_request = new()
            {
                PersonName = "Samuel",
                Email = "onyango@gmail.com",
                DateOfBirth = Convert.ToDateTime("2000-03-05"),
                Address = "Nairobi, Kenya",
                CountryID = country_response_from_add.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
            };

            PersonResponse person_response_from_add =  _personsService.AddPerson(person_request);

            //PersonResponse from get
            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }
        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> persons_from_get = _personsService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Kenya" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_1 = _countryService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countryService.AddCountry(country_request_2);
            List<PersonAddRequest> persons_request_list = [

                new() 
                {
                    PersonName = "Samuel",
                    Email = "onyango@gmail.com",
                    DateOfBirth = Convert.ToDateTime("2000-03-05"),
                    Address = "Nairobi, Kenya",
                    CountryID = country_response_1.CountryID,
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = true,
                },

                new() 
                {
                    PersonName = "Brain",
                    Email = "brian@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1999-06-05"),
                    CountryID = country_response_1.CountryID,
                    Address = "Eldoret, Kenya",
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = false,
                },

                new()
                {
                    PersonName = "Jackline",
                    Email = "Jackline@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1994-12-05"),
                    CountryID = country_response_2.CountryID,
                    Address = "New York City, USA",
                    Gender = GenderOptions.Female,
                    ReceiveNewsLetter = true
                }
           ];

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_request in  persons_request_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_request));
            }

            //person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Get the actual lis
            List<PersonResponse> person_response_list_from_get = _personsService.GetAllPersons();

            //person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in person_response_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (var person_reponse_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_reponse_from_add, person_response_list_from_get);
            }
        }

        #endregion

        #region GetFilteredPersons
        //When the search string is empty all the persons should be returned
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Kenya" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_1 = _countryService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countryService.AddCountry(country_request_2);
            List<PersonAddRequest> persons_request_list = [

                new()
                {
                    PersonName = "Samuel",
                    Email = "onyango@gmail.com",
                    DateOfBirth = Convert.ToDateTime("2000-03-05"),
                    Address = "Nairobi, Kenya",
                    CountryID = country_response_1.CountryID,
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = true,
                },

                new()
                {
                    PersonName = "Brain",
                    Email = "brian@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1999-06-05"),
                    CountryID = country_response_1.CountryID,
                    Address = "Eldoret, Kenya",
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = false,
                },

                new()
                {
                    PersonName = "Jackline",
                    Email = "Jackline@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1994-12-05"),
                    CountryID = country_response_2.CountryID,
                    Address = "New York City, USA",
                    Gender = GenderOptions.Female,
                    ReceiveNewsLetter = true
                }
           ];

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in persons_request_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_request));
            }

            //person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Get the actual lis
            List<PersonResponse> person_response_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in person_response_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (var person_reponse_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_reponse_from_add, person_response_list_from_search);
            }
        }


        //When the search string is supplied, should return all the objects with the matching string
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Kenya" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_1 = _countryService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countryService.AddCountry(country_request_2);
            List<PersonAddRequest> persons_request_list = [

                new()
                {
                    PersonName = "Samuel",
                    Email = "onyango@gmail.com",
                    DateOfBirth = Convert.ToDateTime("2000-03-05"),
                    Address = "Nairobi, Kenya",
                    CountryID = country_response_1.CountryID,
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = true,
                },

                new()
                {
                    PersonName = "Brain",
                    Email = "brian@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1999-06-05"),
                    CountryID = country_response_1.CountryID,
                    Address = "Eldoret, Kenya",
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = false,
                },

                new()
                {
                    PersonName = "Jackline",
                    Email = "Jackline@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1994-12-05"),
                    CountryID = country_response_2.CountryID,
                    Address = "New York City, USA",
                    Gender = GenderOptions.Female,
                    ReceiveNewsLetter = true
                }
           ];

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in persons_request_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_request));
            }

            //person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Get the actual lis
            List<PersonResponse> person_response_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "am");

            //person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in person_response_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (var person_reponse_from_add in person_response_list_from_add)
            {
                if (!string.IsNullOrEmpty(person_reponse_from_add.PersonName))
                {
                    if (person_reponse_from_add.PersonName.Contains("am", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_reponse_from_add, person_response_list_from_search);
                    }
                }
                
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons_SortByPersonName()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Kenya" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_1 = _countryService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countryService.AddCountry(country_request_2);
            List<PersonAddRequest> persons_request_list = [

                new()
                {
                    PersonName = "Samuel",
                    Email = "onyango@gmail.com",
                    DateOfBirth = Convert.ToDateTime("2000-03-05"),
                    Address = "Nairobi, Kenya",
                    CountryID = country_response_1.CountryID,
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = true,
                },

                new()
                {
                    PersonName = "Brain",
                    Email = "brian@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1999-06-05"),
                    CountryID = country_response_1.CountryID,
                    Address = "Eldoret, Kenya",
                    Gender = GenderOptions.Male,
                    ReceiveNewsLetter = false,
                },

                new()
                {
                    PersonName = "Jackline",
                    Email = "Jackline@gmail.com",
                    DateOfBirth = Convert.ToDateTime("1994-12-05"),
                    CountryID = country_response_2.CountryID,
                    Address = "New York City, USA",
                    Gender = GenderOptions.Female,
                    ReceiveNewsLetter = true
                }
           ];

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in persons_request_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_request));
            }

            //person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = _personsService.GetAllPersons();

            //Get the actual lis
            List<PersonResponse> person_response_list_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in person_response_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            person_response_list_from_add = [.. person_response_list_from_add.OrderByDescending(person => person.PersonName)];
            
            //Assert
            for(int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], person_response_list_from_sort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        //When PersonUpdateRequest is null - throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPersonUpdateRequest()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });
        }

        //When the PersonName is null - Throw ArgumentException

        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            CountryAddRequest country_request = new() { CountryName = "Kenya" };
            CountryResponse country_response = _countryService.AddCountry(country_request);

            PersonAddRequest person_add_request = new() 
            {
                PersonName = "Jonathan", 
                CountryID = country_response.CountryID,
                Email = "jona@gmail.com",
                Gender = GenderOptions.Male,
                Address = "23rd Streets, Magadi Road"
            };

            PersonResponse person_repsonse_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_repsonse_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.UpdatePerson(person_update_request);
            });
        }

        //When the Email is null - Throw ArgumentException

        [Fact]
        public void UpdatePerson_EmailIsNull()
        {
            CountryAddRequest country_request = new() { CountryName = "Kenya" };
            CountryResponse country_response = _countryService.AddCountry(country_request);

            PersonAddRequest person_add_request = new() 
            { 
                PersonName = "Jonathan", 
                CountryID = country_response.CountryID, 
                Email = "jona@gmail.com",
                Gender = GenderOptions.Male,
            };

            PersonResponse person_repsonse_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_repsonse_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "Jephren";
            person_update_request.Email = null; 

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.UpdatePerson(person_update_request);
            });
        }

        //When full details is provided - Update occurs

        [Fact]
        public void UpdatePerson_FullPersonDetailsUpdate()
        {
            CountryAddRequest country_request = new() { CountryName = "Kenya" };
            CountryResponse country_response = _countryService.AddCountry(country_request);

            PersonAddRequest person_add_request = new() 
            { 
                PersonName = "Jonathan", 
                Email = "email@gmail.com",
                Address = "24th Street, Rongai",
                DateOfBirth = DateTime.Parse("2000-04-23"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
                CountryID = country_response.CountryID 
            };

            PersonResponse person_repsonse_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_repsonse_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "Jephren";
            person_update_request.Email = "niccowilliams@gmail.com";

            PersonResponse? person_response_from_update = _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_update?.PersonID);

            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            CountryAddRequest country_request = new() { CountryName = "Japan" };
            CountryResponse country_response = _countryService.AddCountry(country_request);

            PersonAddRequest person_request = new()
            { 
                PersonName = "Samuel",
                Email = "sammy@gmail.com",
                Address = "25th Street, Magadi Road",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1999/03/23"),
                ReceiveNewsLetter = true,
                CountryID = country_response.CountryID
            };

            PersonResponse person_response = _personsService.AddPerson(person_request);

            //Act
            bool isDeleted = _personsService.DeletePerson(person_response.PersonID);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            CountryAddRequest country_request = new() { CountryName = "Japan" };
            CountryResponse country_response = _countryService.AddCountry(country_request);

            PersonAddRequest person_request = new()
            {
                PersonName = "Samuel",
                Email = "sammy@gmail.com",
                Address = "25th Street, Magadi Road",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1999/03/23"),
                ReceiveNewsLetter = true,
                CountryID = country_response.CountryID
            };

            PersonResponse person_response = _personsService.AddPerson(person_request);

            //Act
            bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }
        #endregion
    }
}
