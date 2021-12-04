using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class LtcPharmacyDto
    {
        public LtcPharmacyDto(LtcPharmacyId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public LtcPharmacyId Id { get; set; }
        public string? Name { get; set; }
    }
}
