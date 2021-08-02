using System.Drawing;
using System.Threading;

namespace RecordNplay
{
    public class WaitColorEvent : MacroEvent
    {
        public Color color;
        public int x;
        public int y;
        public bool contrary;
        private Bitmap bmp;
        public WaitColorEvent()
        {
            this.startTime = -1;
            color = Color.FromArgb(0,0,0);
            contrary = false;
            x = -1;
            y = -1;
            bmp = new Bitmap(1, 1);
        }
        public WaitColorEvent(int startTime,int red, int green, int blue, int x, int y, bool contrary=false)
        {
            this.startTime = startTime;
            color = Color.FromArgb(red, green, blue);
            this.x = x;
            this.y = y;
            bmp = new Bitmap(1, 1);
            this.contrary = contrary;
        }
        
        Color GetColorAt(int x, int y)
        {
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }
        public override void activate()
        {
            while(Form1.running) // check if still need to wait (user didn't press Escape to stop the macro)
            {
                Color currentColor = GetColorAt(x, y);
                if (contrary) // wait for the color NOT to be the color we chose
                {
                    if (!currentColor.Equals(color))
                    {
                        break;
                    }
                }
                else // wait for the color TO BE the color we chose
                {
                    if (currentColor.Equals(color))
                    {
                        break;
                    }
                }
                new ManualResetEvent(false).WaitOne(1); // do sleep for 1 millisecond to not waste CPU (busy waiting)
            }
        }

        public override string ToString()
        {
            if (contrary)
            {
                return "Wait pixel " + x + "," + y + " NOT to be " + color.R + "," + color.G + "," + color.B + " at " + startTime;
            }
            else
            {
                return "Wait pixel " + x + "," + y + " to be " + color.R + "," + color.G + "," + color.B + " at " + startTime;
            }
        }
    }
}
