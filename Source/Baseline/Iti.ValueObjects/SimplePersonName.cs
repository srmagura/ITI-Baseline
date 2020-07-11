using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.ValueObjects;
using Iti.Baseline.Utilities;
using Newtonsoft.Json;

namespace Iti.Baseline.ValueObjects
{
    public class SimplePersonName : ValueObject<SimplePersonName>
    {
        protected SimplePersonName() { }

        [JsonConstructor]
        public SimplePersonName(string first, string middle, string last, string prefix = null)
        {
            Prefix = prefix?.Trim().MaxLength(FieldLengths.SimplePersonName.Prefix);
            First = first?.Trim().MaxLength(FieldLengths.SimplePersonName.First);
            Middle = middle?.Trim().MaxLength(FieldLengths.SimplePersonName.Middle);
            Last = last?.Trim().MaxLength(FieldLengths.SimplePersonName.Last);
        }

        //

        [MaxLength(FieldLengths.SimplePersonName.Prefix)]
        public string Prefix { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.First)]
        public string First { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.Middle)]
        public string Middle { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.Last)]
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