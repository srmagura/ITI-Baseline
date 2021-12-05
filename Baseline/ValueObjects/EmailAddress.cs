using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.DDD.Core;
using ITI.DDD.Domain;

namespace ITI.Baseline.ValueObjects;

public record EmailAddress : DbValueObject
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            if (!email.HasValue())
                return false;

            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public EmailAddress(string value)
    {
        Value = value.Trim();

        Require.IsTrue(IsValidEmail(Value), $"Invalid email address: {Value}.");
    }

    [MaxLength(FieldLengths.EmailAddress.Value)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
