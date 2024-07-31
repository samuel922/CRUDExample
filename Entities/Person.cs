﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// Defines the business logic for person entity
    /// </summary>
    public class Person
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email {  get; set; }
        public Guid? CountryID { get; set; }
        public string? Country {  get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
    }
}
