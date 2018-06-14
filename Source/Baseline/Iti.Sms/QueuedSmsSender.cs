using System;
using Iti.Sms;

namespace Iti.Sms
{
    public class QueuedSmsSender : ISmsSender
    {
        private readonly ISmsRepository _repo;

        public QueuedSmsSender(ISmsRepository repo)
        {
            _repo = repo;
        }

        public void Send(string toSmsAddress, string body)
        {
            var rec = new SmsRecord(toSmsAddress, body);
            _repo.Add(rec);
        }
    }
}