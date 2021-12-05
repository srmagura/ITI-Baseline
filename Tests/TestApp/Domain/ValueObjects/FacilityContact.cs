using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain;

namespace TestApp.Domain.ValueObjects;

public record FacilityContact : DbValueObject
{
    [Obsolete("Persistence use only")]
    protected FacilityContact() { }

    public FacilityContact(PersonName? name, EmailAddress? email)
    {
        Name = name;
        Email = email;
    }

    public PersonName? Name { get; protected init; }
    public EmailAddress? Email { get; protected init; }
}
