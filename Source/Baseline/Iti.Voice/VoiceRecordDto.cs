using System;
using Iti.Core.DTOs;
using Iti.Identities;

namespace Iti.Voice
{
    public class VoiceRecordDto : IDto
    {
        public VoiceRecordId Id { get; set; } = new VoiceRecordId();

        public NotificationId NotificationId { get; set; }

        public VoiceStatus Status { get; set; } = VoiceStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; set; }

        public string Body { get; set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}