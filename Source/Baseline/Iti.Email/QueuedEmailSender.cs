﻿using System;
using Iti.Identities;

namespace Iti.Email
{
    public class QueuedEmailSender : IEmailSender
    {
        private readonly IEmailRepository _repo;

        public QueuedEmailSender(IEmailRepository repo)
        {
            _repo = repo;
        }

        public void Send(NotificationId notificationId, string toEmailAddress, string subject, string body)
        {
            var rec = new EmailRecord(notificationId, toEmailAddress, subject, body);
            _repo.Add(rec);
        }
    }
}