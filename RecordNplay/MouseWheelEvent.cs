using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    class MouseWheelEvent : PressedMouseEvent
    {
        public int delta;


        public MouseWheelEvent() : base()
        {
            delta = 0;
        }

        public MouseWheelEvent(byte clickType, int x, int y, long startTime, int delta) : base(clickType,  x,  y, startTime)
        {
            this.delta = delta;
        }

        public MouseWheelEvent(MouseWheelEvent pressedInfo) : base(pressedInfo)
        {
            delta = pressedInfo.delta;
        }

        public override void activate()
        {
            if (clickType == 6)
            {
                MouseClicker.scrollMouse(x, y, delta);
            }
        }

        public override string ToString()
        {
            if (clickType == 6)//leftClickDown
            {
                if (delta > 0)
                {
                    return "Scroll " + delta + " Up{" + this.x + "," + this.y + "}  at" + startTime;
                }
                else if (delta < 0)
                {
                    return "Scroll " + delta*-1 + " Down{" + this.x + "," + this.y + "} at" + startTime;
                }
            }
            
            return "Error";
        }
    }
}
