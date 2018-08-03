using System;
using Iti.Core.Entites;

namespace Iti.Voice
{
    public class VoiceRecord : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected VoiceRecord() { }

        public VoiceRecord(string toAddress, string callbackUrl, string body)
        {
            ToAddress = toAddress;
            Body = body;
            CallbackUrl = callbackUrl;
        }

        //

        public long Id { get; set; }

        public VoiceStatus Status { get; set; } = VoiceStatus.Pending;

        public DateTimeOffset? SentUtc { get; set; }

        public string ToAddress { get; protected set; }

        public string CallbackUrl { get; protected set; }

        public string Body { get; protected set; }

        public int RetryCount { get; set; }
        public DateTimeOffset? NextRetry { get; set; }
    }
}