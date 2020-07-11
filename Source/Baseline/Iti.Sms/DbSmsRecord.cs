using System;
using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.DataContext;

namespace Iti.Baseline.Sms
{
    public class DbSmsRecord : DbEntity
    {
        [Obsolete("Serialization use only")]
        protected DbSmsRecord() { }

        public DbSmsRecord(Guid? notificationId, string toAddress, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Body = body;
        }

        //

        public Guid? NotificationId { get; protected set; }

        public SmsStatus Status { get; set; } = SmsStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        [MaxLength(32)]
        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}