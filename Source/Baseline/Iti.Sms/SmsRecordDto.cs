using System;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.Identities;

namespace Iti.Baseline.Sms
{
    public class SmsRecordDto : IDto
    {
        public SmsRecordId Id { get; set; }

        public NotificationId NotificationId { get; set; }

        public SmsStatus Status { get; set; } = SmsStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; set; }

        public string Body { get; set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}