using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.Entites;
using Iti.Utilities;

namespace Iti.Email
{
    public class EmailRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected EmailRecord() { }

        public EmailRecord(string toAddress, string subject, string body)
        {
            ToAddress = toAddress;
            Subject = subject.MaxLength(512);
            Body = body;
        }

        //

        public long Id { get; set; }

        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        [MaxLength(512)]
        public string Subject { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}