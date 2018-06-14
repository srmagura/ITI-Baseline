using System.ComponentModel.DataAnnotations;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class PersonName : ValueObject<PersonName>
    {
        protected PersonName() { }

        public PersonName(string first, string middle, string last, string prefix = null)
        {
            Prefix = prefix?.Trim().MaxLength(FieldLengths.PersonName.Prefix);
            First = first?.Trim().MaxLength(FieldLengths.PersonName.First);
            Middle = middle?.Trim().MaxLength(FieldLengths.PersonName.Middle);
            Last = last?.Trim().MaxLength(FieldLengths.PersonName.Last);
        }

        //

        [MaxLength(FieldLengths.PersonName.Prefix)]
        public string Prefix { get; protected set; }

        [MaxLength(FieldLengths.PersonName.First)]
        public string First { get; protected set; }

        [MaxLength(FieldLengths.PersonName.Middle)]
        public string Middle { get; protected set; }

        [MaxLength(FieldLengths.PersonName.Last)]
        public string Last { get; protected set; }

        public override string ToString()
        {
            var s = $"{Prefix ?? ""} {Last}, {First} {Middle}";
            return s.Trim();
        }

        public override bool HasValue()
        {
            return HasAnyValue(Prefix, First, Middle, Last);
        }
    }
}