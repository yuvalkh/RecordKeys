using System;
using System.Diagnostics;

namespace RecordNplay
{
    public class EditableStopWatch : Stopwatch
    {
        public TimeSpan StartOffset { get; private set; }

        public EditableStopWatch(long startTimeInMilliseconds)
        {
            StartOffset = new TimeSpan(startTimeInMilliseconds * 10000);
        }

        public new long ElapsedMilliseconds
        {
            get
            {
                return base.ElapsedMilliseconds + (long)StartOffset.TotalMilliseconds;
            }
        }

        public new long ElapsedTicks
        {
            get
            {
                return base.ElapsedTicks + StartOffset.Ticks;
            }
        }

    }
}
