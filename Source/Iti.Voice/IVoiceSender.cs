﻿namespace Iti.Voice
{
    public interface IVoiceSender
    {
        void Send(string toPhoneNumber, string callbackUrl, string content);
    }
}