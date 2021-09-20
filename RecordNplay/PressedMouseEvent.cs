using System;

namespace RecordNplay
{
    [Serializable]
    public class PressedMouseEvent : MacroEvent
    {
        public byte clickType;
        public int x;
        public int y;

        public PressedMouseEvent()
        {
            clickType = 1;
            x = -1;
            y = -1;
            startTime = -1;
        }

        public PressedMouseEvent(byte clickType, int x, int y,long startTime)
        {
            this.clickType = clickType;
            this.x = x;
            this.y = y;
            this.startTime = startTime;
        }

        public PressedMouseEvent(PressedMouseEvent pressedInfo)
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
            else if (clickType == 4)
            {
                MouseClicker.pressMiddleMouse(x, y);
            }
            else if (clickType == 5)
            {
                MouseClicker.leaveMiddletMouse(x, y);
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
            else if (clickType == 4)//rightClickUp
            {
                return "Middle Click Down{" + this.x + "," + this.y + "} at" + startTime;
            }
            else if (clickType == 5)//rightClickUp
            {
                return "Middle Click Up{" + this.x + "," + this.y + "} at" + startTime;
            }
            return "Error";
        }
    }
}
