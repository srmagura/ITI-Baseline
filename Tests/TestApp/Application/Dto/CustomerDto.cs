using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class CustomerDto
    {
        public CustomerDto(CustomerId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public CustomerId Id { get; set; }
        public string? Name { get; set; }

        public AddressDto? Address { get; set; }
        public PersonNameDto? ContactName { get; set; }
        public PhoneNumberDto? ContactPhone { get; set; }

        public List<LtcPharmacyDto> LtcPharmacies { get; set; } = new List<LtcPharmacyDto>();
        public List<int> SomeInts { get; set; } = new List<int>();

        public decimal SomeMoney { get; set; }
        public long SomeNumber { get; set; }
    }
}
