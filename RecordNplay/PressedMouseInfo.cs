using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordNplay
{
    [Serializable]
    public class PressedMouseInfo : PressedInput
    {
        public byte clickType;
        public int x;
        public int y;

        public PressedMouseInfo()
        {
            this.clickType = 1;
            this.x = -1;
            this.y = -1;
            this.startTime = -1;
        }

        public PressedMouseInfo(byte clickType, int x, int y,long startTime)
        {
            this.clickType = clickType;
            this.x = x;
            this.y = y;
            this.startTime = startTime;
        }

        public PressedMouseInfo(PressedMouseInfo pressedInfo)
        {
            clickType = pressedInfo.clickType;
            x = pressedInfo.x;
            y = pressedInfo.y;
            startTime = pressedInfo.startTime;
        }

        public override void activate()
        {
            if (clickType == 0)
            {
                MouseClicker.pressLeftMouse(x, y);
            }
            else if (clickType == 1)
            {
                MouseClicker.leaveLeftMouse(x, y);
            }
            else if (clickType == 2)
            {
                MouseClicker.pressRightMouse(x, y);
            }
            else if (clickType == 3)
            {
                MouseClicker.leaveRighttMouse(x, y);
            }
        }

        public override string ToString()
        {
            if(clickType == 0)//leftClickDown
            {
                return "Left Click Down{" + this.x + "," + this.y + "} at" + startTime;
            } else if(clickType == 1)//leftClickUp
            {
                return "Left Click Up{" + this.x + "," + this.y + "} at" + startTime;
            }
            else if(clickType == 2)//rightClickDown
            {
                return "Right Click Down{" + this.x + "," + this.y + "} at" + startTime;
            }
            else if(clickType == 3)//rightClickUp
            {
                return "Right Click Up{" + this.x + "," + this.y + "} at" + startTime;
            }
            return "Error";
        }
    }
}
