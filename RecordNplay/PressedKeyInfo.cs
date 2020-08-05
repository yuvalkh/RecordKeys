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
        public byte keyCode { get; set; }
        public long duration { get; set; }

        public PressedKeyInfo()
        {
            this.keyCode = 0;
            this.duration = 0;
            this.startTime = 0;
        }
        public PressedKeyInfo(byte keyCode, long duration, long startTime)
        {
            this.keyCode = keyCode;
            this.duration = duration;
            this.startTime = startTime;
        }

        public PressedKeyInfo(PressedKeyInfo pressedInfo)
        {
            keyCode = pressedInfo.keyCode;
            duration = pressedInfo.duration;
            startTime = pressedInfo.startTime;
        }

        public override void activate()
        {
            new Task(() =>
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
