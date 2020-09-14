using Iti.Baseline.Core.Entities;
using Iti.Baseline.ValueObjects;

namespace Domain
{
    public class ValObjHolder : AggregateRoot
    {
        public ValObjHolderId Id { get; protected set; } = new ValObjHolderId();

        public string Name { get; set; }

        public SimpleAddress SimpleAddress { get; set; }
        public SimplePersonName SimplePersonName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }

        public ValueParent ValueParent { get; set; }
    }
}