using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Contains all the properties needed to create a Person (Excluding the ID)
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name should be supplied.")]
        public string? PersonName { get; set; }
        [EmailAddress(ErrorMessage = "Email value should be a valid email address.")]
        [Required(ErrorMessage = "Email must be provided.")]
        public string? Email {  get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? CountryID { get; set; }
        public GenderOptions? Gender { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                CountryID = CountryID,
                Gender = Gender.ToString(),
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }
}
