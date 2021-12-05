using ITI.DDD.Domain;

namespace TestApp.Domain.Identities;

public record LtcPharmacyId : Identity
{
    public LtcPharmacyId() { }
    public LtcPharmacyId(Guid guid) : base(guid) { }
    public LtcPharmacyId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
