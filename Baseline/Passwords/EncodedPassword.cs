using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.Passwords;

public record EncodedPassword : DbValueObject
{
    public EncodedPassword(string value)
    {
        Require.HasValue(value, "Invalid encoded password (empty).");

        Value = value;
    }

    [MaxLength(128)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
