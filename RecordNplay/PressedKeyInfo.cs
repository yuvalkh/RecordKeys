using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public override void activate()
        {
            new Thread(() =>
            {
                KeysWriter.holdKey(keyCode, (int)duration);
            }).Start();
        }

        public override string ToString()
        {
            return "Pressed " + ((Keys)keyCode).ToString() + " for " + duration + " time at " + startTime;
        }
    }
}
