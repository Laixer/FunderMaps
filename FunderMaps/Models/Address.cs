using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models
{
    public class Address
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int AddressNumber { get; set; }

        public string AddressNumberPostfix { get; set; }
        public string City { get; set; }
        public string Postbox { get; set; }
        public string Zipcode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public void Reassign(Address other)
        {
            Street = other.Street;
            AddressNumber = other.AddressNumber;
            AddressNumberPostfix = other.AddressNumberPostfix;
            City = other.City;
            Postbox = other.Postbox;
            Zipcode = other.Zipcode;
            State = other.State;
            Country = other.Country;
        }
    }
}
