using System;
using Iti.Core.DTOs;
using Iti.Identities;

namespace Iti.Sms
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