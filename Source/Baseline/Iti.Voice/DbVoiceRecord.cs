using System;
using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.DataContext;

namespace Iti.Baseline.Voice
{
    public class DbVoiceRecord : DbEntity
    {
        [Obsolete("Serialization use only")]
        protected DbVoiceRecord() { }

        public DbVoiceRecord(Guid? notificationId, string toAddress, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Body = body;
        }

        //

        public Guid? NotificationId { get; set; }

        public VoiceStatus Status { get; set; } = VoiceStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        [MaxLength(64)]
        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}