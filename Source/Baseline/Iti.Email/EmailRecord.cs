﻿using System;
using Iti.Core.Entites;
using Iti.Identities;
using Iti.Utilities;

namespace Iti.Email
{
    public class EmailRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected EmailRecord() { }

        public EmailRecord(NotificationId notificationId, string toAddress, string subject, string body)
        {
            NotificationId = notificationId;
            ToAddress = toAddress;
            Subject = subject.MaxLength(512);
            Body = body;
        }

        //

        public EmailRecordId Id { get; set; } = new EmailRecordId();

        public NotificationId NotificationId { get; protected set; }

        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Subject { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetryUtc { get; set; }
    }
}