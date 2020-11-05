﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Baseline.Util
{
    public class DebugTimer
    {
        private readonly string _name;
        private readonly DateTime _start;
        private readonly Action<string, TimeSpan> _output;

        public DebugTimer(string name, Action<string, TimeSpan> output = null)
        {
            _name = name;
            _start = DateTime.Now;

            if (output == null)
                output = (tag, ts) => { Console.WriteLine($"{tag}: {ts}"); };
            _output = output;
        }

        public void Dispose()
        {
            var end = DateTime.Now;
            var diff = end - _start;

            _output?.Invoke(_name, diff);
        }
    }
}