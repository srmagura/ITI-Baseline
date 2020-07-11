using System;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
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