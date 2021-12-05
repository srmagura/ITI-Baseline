using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using ITI.DDD.Domain;
using TestApp.Domain.Events;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Domain;

public class Customer : AggregateRoot
{
    [Obsolete("Only for use by AutoMapper and other constructors.")]
    public Customer(
        bool placeholder,
        string name,
        List<LtcPharmacy> ltcPharmacies,
        List<int> someInts,
        long someNumber
    )
    {
        SetName(name);
        _ltcPharmacies = ltcPharmacies;
        _someInts = someInts;
        SomeNumber = someNumber;
    }

    public Customer(
        string name,
        List<LtcPharmacy> ltcPharmacies,
        List<int> someInts,
        long someNumber
#pragma warning disable CS0618 // Type or member is obsolete
    ) : this(placeholder: true, name, ltcPharmacies, someInts, someNumber)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        Raise(new CustomerAddedEvent(Id, name));
    }

    //
    // IDENTITY
    //

    public CustomerId Id { get; protected set; } = new CustomerId();

    //
    // ATTRIBUTES
    //

    public string? Name { get; protected set; }

    public Address? Address { get; protected set; }
    public PersonName? ContactName { get; protected set; }
    public PhoneNumber? ContactPhone { get; protected set; }

    public decimal SomeMoney { get; protected set; }
    public long SomeNumber { get; protected set; }

    //
    // RELATIONSHIPS
    //

    private readonly List<LtcPharmacy> _ltcPharmacies;
    public IReadOnlyCollection<LtcPharmacy> LtcPharmacies => _ltcPharmacies;

    private readonly List<int> _someInts;
    public IReadOnlyCollection<int> SomeInts => _someInts;

    //
    // OPERATIONS
    //

    public void SetName(string newName)
    {
        Name = newName ?? throw new ValidationException("Name");
    }

    public void SetAddress(Address? address)
    {
        Address = address;
    }

    public void SetContact(PersonName? contactName, PhoneNumber? contactPhone)
    {
        ContactName = contactName;
        ContactPhone = contactPhone;
    }

    public void AddLtcPharmacy(string name)
    {
        _ltcPharmacies.Add(new LtcPharmacy(name));
    }

    public void RenameLtcPharmacy(LtcPharmacyId id, string name)
    {
        _ltcPharmacies.Single(p => p.Id == id).SetName(name);
    }

    public void RemoveLtcPharmacy(LtcPharmacyId id)
    {
        _ltcPharmacies.RemoveAll(p => p.Id == id);
    }
}
