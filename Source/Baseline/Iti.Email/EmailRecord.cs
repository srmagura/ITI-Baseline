using System;
using Iti.Baseline.Core.Entities;
using Iti.Baseline.Identities;
using Iti.Baseline.Utilities;

namespace Iti.Baseline.Email
{
    public class EmailRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected EmailRecord() { }

        public EmailRecord(NotificationId notificationId, string toAddress, string subject, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Subject = subject.MaxLength(512);
            Body = body;
        }

        //

        public EmailRecordId Id { get; set; } = new EmailRecordId();

        public NotificationId NotificationId { get; protected set; }

        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Subject { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}