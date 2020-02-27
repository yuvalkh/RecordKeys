using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    [Serializable]
    public class PressedKeyInfo : PressedInput
    {
        public byte keyCode;
        public long duration;

        public PressedKeyInfo(byte keyCode, long duration, long startTime)
        {
            this.keyCode = keyCode;
            this.duration = duration;
            this.startTime = startTime;
        }

        public override string ToString()
        {
            return "Still not implemented !";
        }
    }
}
