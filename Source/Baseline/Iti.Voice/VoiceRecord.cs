using System;
using Iti.Core.Entites;

namespace Iti.Voice
{
    public class VoiceRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected VoiceRecord() { }

        public VoiceRecord(long? notificationId, string toAddress, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Body = body;
        }

        //

        public long Id { get; set; }

        public long? NotificationId { get; set; }

        public VoiceStatus Status { get; set; } = VoiceStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}