using ITI.DDD.Core;
using ITI.DDD.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Domain;

public class LtcPharmacy : Member<Customer>
{
    public LtcPharmacy(string name)
    {
        SetName(name);
    }

    public LtcPharmacyId Id { get; protected set; } = new LtcPharmacyId();

    public string? Name { get; protected set; }

    internal void SetName(string name)
    {
        Name = name ?? throw new ValidationException("Name");
    }
}
