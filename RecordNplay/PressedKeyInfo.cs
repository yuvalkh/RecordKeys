using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    public class PressedKeyInfo : PressedInput
    {
        public byte keyCode;
        public long duration;
        public long startTime;

        public PressedKeyInfo(byte keyCode, long duration, long startTime)
        {
            this.keyCode = keyCode;
            this.duration = duration;
            this.startTime = startTime;
        }
    }
}
