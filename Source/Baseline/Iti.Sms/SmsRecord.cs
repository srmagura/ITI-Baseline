using System;
using Iti.Core.Entites;
using Iti.Identities;

namespace Iti.Sms
{
    public class SmsRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected SmsRecord() { }

        public SmsRecord(NotificationId notificationId, string toAddress, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Body = body;
        }

        //

        public SmsRecordId Id { get; set; } = new SmsRecordId();

        public NotificationId NotificationId { get; protected set; }

        public SmsStatus Status { get; set; } = SmsStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}