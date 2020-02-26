using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    public class PressedMouseInfo : PressedInput
    {
        public byte keyCode;
        public long duration;
        public long startTime;

        public PressedMouseInfo(byte keyCode, long duration, long startTime)
        {
            this.keyCode = keyCode;
            this.duration = duration;
            this.startTime = startTime;
        }
    }
}
