using System;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.Identities;

namespace Iti.Baseline.Email
{
    public class EmailRecordDto : IDto
    {
        public EmailRecordId Id { get; set; }

        public NotificationId NotificationId { get; set; }

        public EmailStatus Status { get; set; }

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}