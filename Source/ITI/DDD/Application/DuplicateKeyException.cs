using ITI.DDD.Core;
using System;

namespace ITI.DDD.Application;

public class DuplicateKeyException : DomainException
{
    public DuplicateKeyException(string table, string value)
        : base($"Duplicate key: {table} value '{value}'", AppServiceLogAs.None)
    {
        Table = table;
        Value = value;
    }

    public DuplicateKeyException(string table, string value, Exception innerException)
        : base($"Duplicate key: {table} value '{value}'", innerException, AppServiceLogAs.None)
    {
        Table = table;
        Value = value;
    }

    public string Table { get; protected init; }
    public string Value { get; protected init; }
}
