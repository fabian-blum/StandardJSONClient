using System;

namespace JSONClientSampleCore
{
    public enum Gender
    {
        Male,
        Female,
        None
    }

    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ExternalId { get; set; }
        public DateTime Created { get; set; }
    }
}

