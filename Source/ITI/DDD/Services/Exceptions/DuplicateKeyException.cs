using ITI.DDD.Core.Exceptions;
using System;

namespace ITI.DDD.Services.Exceptions
{
    public class DuplicateKeyException : DomainException
    {
        public DuplicateKeyException(string table, string value)
            : base($"Duplicate Key: {table} value '{value}'", AppServiceLogAs.None)
        {
            Table = table;
            Value = value;
        }

        public DuplicateKeyException(string table, string value, Exception innerException)
            : base($"Duplicate Key: {table} value '{value}'", innerException, AppServiceLogAs.None)
        {
            Table = table;
            Value = value;
        }

        public string Table { get; protected set; }
        public string Value { get; protected set; }
    }
}