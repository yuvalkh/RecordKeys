using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordNplay
{
    public class DetectImage
    {
        public static void detect()
        {
            int calScreenWidth = 1920;
            int calScreenHeight = 1080;
            Bitmap bm = new Bitmap(calScreenWidth, calScreenHeight);
            
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            Mat game_img = bm.ToMat();
            Mat button_img = CvInvoke.Imread("C:/Users/Home/Desktop/Capture.PNG", ImreadModes.Unchanged);
            Mat result = new Mat();
            double minVal = 0;
            double maxVal = 0;
            Point maxLoc = new Point();
            Point minLoc = new Point();
            CvInvoke.MatchTemplate(game_img, button_img, result, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            double threshold = 0.8;

            if (maxVal > threshold)
            {
                Console.WriteLine("I found it at: " + maxLoc);
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
            Thread.Sleep(1000);
        }
    }
}