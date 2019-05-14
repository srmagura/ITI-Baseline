using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.DataContext;
using Iti.Utilities;

namespace Iti.Email
{
    public class DbEmailRecord : DbEntity
    {
        [Obsolete("Serialization use only")]
        protected DbEmailRecord() { }

        public DbEmailRecord(Guid? notificationId, string toAddress, string subject, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Subject = subject.MaxLength(512);
            Body = body;
        }

        //

        public Guid Id { get; set; }

        public Guid? NotificationId { get; protected set; }

        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        [MaxLength(256)]
        public string ToAddress { get; protected set; }

        [MaxLength(1024)]
        public string Subject { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}