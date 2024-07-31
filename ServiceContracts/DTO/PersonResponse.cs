﻿using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email {  get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country {  get; set; }
        public bool ReceiveNewsLetter { get; set; }

        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return PersonID == person.PersonID && 
                PersonName == person.PersonName &&
                Email == person.Email 
                && DateOfBirth == person.DateOfBirth && 
                Address == person.Address &&
                Gender == person.Gender && 
                CountryID == person.CountryID && 
                Country == person.Country &&
                ReceiveNewsLetter == person.ReceiveNewsLetter && 
                Age == person.Age;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"PersonID: {PersonID}, PersonName: {PersonName}, Email: {Email}, DateOfBirth: {DateOfBirth}, " +
                $"Address: {Address}, Gender: {Gender}, CountryID: {CountryID}, Country: {Country}, ReceiveNewsLetter: {ReceiveNewsLetter}, " +
                $"Age: {Age}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                CountryID = CountryID,
                ReceiveNewsLetter = ReceiveNewsLetter,
                
            };
        }


    }

    public static class PersonExtentions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Address = person.Address,
                Gender = person.Gender,
                CountryID = person.CountryID,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Age = (person.DateOfBirth != null ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null)
            };
        }

       
    }
}
