
namespace FunderMaps.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int AddressNumber { get; set; }
        public string AddressNumberPostfix { get; set; }
        public string City { get; set; }
        public string Postbox { get; set; }
        public string Zipcode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
