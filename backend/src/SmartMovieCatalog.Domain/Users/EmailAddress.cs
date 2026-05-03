using System.Net.Mail;
using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Users;

public sealed class EmailAddress : ValueObject
{
    private EmailAddress(string value)
    {
        Value = value;
        NormalizedValue = Normalize(value);
    }

    public string Value { get; }

    public string NormalizedValue { get; }

    public static EmailAddress Create(string value)
    {
        string trimmed = Guard.AgainstNullOrWhiteSpace(value, nameof(value)).Trim();

        if (trimmed.Length > 320 || !IsValid(trimmed))
        {
            throw new ArgumentException("Email address is invalid.", nameof(value));
        }

        return new EmailAddress(trimmed);
    }

    public static bool TryCreate(string? value, out EmailAddress? emailAddress)
    {
        emailAddress = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        string trimmed = value.Trim();
        if (trimmed.Length > 320 || !IsValid(trimmed))
        {
            return false;
        }

        emailAddress = new EmailAddress(trimmed);
        return true;
    }

    public static string Normalize(string value)
    {
        return Guard.AgainstNullOrWhiteSpace(value, nameof(value)).Trim().ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return NormalizedValue;
    }

    public override string ToString() => Value;

    private static bool IsValid(string value)
    {
        try
        {
            return new MailAddress(value).Address.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
