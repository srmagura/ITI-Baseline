using System;

namespace Iti.Email
{
    public class QueuedEmailSender : IEmailSender
    {
        private readonly IEmailRepository _repo;

        public QueuedEmailSender(IEmailRepository repo)
        {
            _repo = repo;
        }

        public void Send(string toEmailAddress, string subject, string body)
        {
            var rec = new EmailRecord(toEmailAddress, subject, body);
            _repo.Add(rec);
        }
    }
}