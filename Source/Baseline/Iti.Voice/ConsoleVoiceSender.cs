﻿using System;

namespace Iti.Voice
{
    public class ConsoleVoiceSender : IVoiceSender
    {
        public void Send(string toPhoneNumber, string content)
        {
            Console.WriteLine($"=== Voice: {toPhoneNumber} ===============================");
            Console.WriteLine($"{content}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
