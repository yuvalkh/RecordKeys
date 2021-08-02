using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordNplay
{
    [Serializable]
    public class PressedKeyEvent : MacroEvent
    {
        public byte keyCode { get; set; }
        public long duration { get; set; }

        public PressedKeyEvent()
        {
            this.keyCode = 0;
            this.duration = 0;
            this.startTime = 0;
        }
        public PressedKeyEvent(byte keyCode, long duration, long startTime)
        {
            this.keyCode = keyCode;
            this.duration = duration;
            this.startTime = startTime;
        }

        public PressedKeyEvent(PressedKeyEvent pressedInfo)
        {
            keyCode = pressedInfo.keyCode;
            duration = pressedInfo.duration;
            startTime = pressedInfo.startTime;
        }

        public override void activate()
        {
            if (keyCode == 27) // bytecode of Esc
            {
                KeysClicker.clickEscape++;
            }
            new Task(() =>
            {
                KeysClicker.holdKey(keyCode, (int)duration);
            }).Start();
        }

        public override string ToString()
        {
            return "Press " + ((Keys)keyCode).ToString() + " for " + duration + " time at " + startTime;
        }
    }
}
