using System;
using Iti.Core.Entites;

namespace Iti.Sms
{
    public class SmsRecord : AggregateRoot
    {
        [Obsolete("Persistence use only")]
        protected SmsRecord() { }

        public SmsRecord(string toAddress, string body)
        {
            ToAddress = toAddress;
            Body = body;
        }

        //

        public long Id { get; set; }

        public SmsStatus Status { get; set; } = SmsStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetry { get; set; }
    }
}