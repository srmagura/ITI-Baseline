using System;
using Iti.Baseline.Core.Entities;
using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public class VoiceRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected VoiceRecord() { }

        public VoiceRecord(NotificationId notificationId, string toAddress, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Body = body;
        }

        //

        public VoiceRecordId Id { get; set; } = new VoiceRecordId();

        public NotificationId NotificationId { get; set; }

        public VoiceStatus Status { get; set; } = VoiceStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}