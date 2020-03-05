using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
