using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordNplay
{
    class ClickOnImageEvent : MacroEvent
    {
        public string imgName; // for better display
        public string img; // saved that in purpose to be string because saving Bitmap with Newtonsoft dll is kinda bad
        public float threshold;
        public ClickOnImageEvent()
        {
            this.imgName = null;
            this.startTime = -1;
            this.img = null;
            this.threshold = -1;
        }
        public ClickOnImageEvent(int startTime, string img, float threshold, string imgName)
        {
            this.imgName = imgName;
            this.startTime = startTime;
            this.img = img;
            this.threshold = threshold;
        }
        public override void activate()
        {
            Bitmap imgAsBitmap = TextDialog.ByteStringToBitmap(img);
            while (Form1.running) // check if still need to wait (user didn't press Escape to stop the macro)
            {
                // still need to understand how to get the width and height instead of fixed numbers
                var rc = Screen.PrimaryScreen.Bounds;
                int calScreenWidth = rc.Width;
                int calScreenHeight = rc.Height;
                //get a scrennshot of the pc
                Bitmap bm = new Bitmap(calScreenWidth, calScreenHeight);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);

                Mat game_img = bm.ToMat();
                Mat button_img = imgAsBitmap.ToMat();
                Mat result = new Mat();
                double minVal = 0;
                double maxVal = 0;
                Point maxLoc = new Point();
                Point minLoc = new Point();
                //search for matches and then take the highest
                CvInvoke.MatchTemplate(game_img, button_img, result, TemplateMatchingType.CcoeffNormed);
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                if (maxVal > threshold)
                { 
                    //when we found, we click on the image (middle of the image)
                    MouseClicker.pressLeftMouse(maxLoc.X + button_img.Width / 2, maxLoc.Y + button_img.Height / 2);
                    MouseClicker.leaveLeftMouse(maxLoc.X + button_img.Width / 2, maxLoc.Y + button_img.Height / 2);
                    break;
                }
                else
                {
                    Console.WriteLine("Didn't found that");
                }
                game_img.Dispose();
                button_img.Dispose();
                result.Dispose();
                bm.Dispose();
                g.Dispose();
                new ManualResetEvent(false).WaitOne(100);
            }
        }

        public override string ToString()
        {
            return "Wait screen to have the picture " + imgName + " at " + startTime;
        }
    }
}
