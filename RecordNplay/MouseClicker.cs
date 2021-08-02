using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace RecordNplay
{
    class MouseClicker
    {

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_MOVE = 0x0001;


        public static void pressLeftMouse(int x, int y)
        {
            if(Cursor.Position.X != x || Cursor.Position.Y != y)
            {
                Cursor.Position = new Point(x, y);
            }
            //mouse_event(MOUSEEVENTF_MOVE, (uint)x, (uint)y, 0, new UIntPtr(0));
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new UIntPtr(0));
        }

        public static void LinearSmoothMove(Point newPosition, int steps)
        {
            Point start = Cursor.Position;
            PointF iterPoint = start;

            // Find the slope of the line segment defined by start and newPosition
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);

            // Divide by the number of steps
            slope.X = slope.X / steps;
            slope.Y = slope.Y / steps;

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                SetCursorPos(Point.Round(iterPoint).X, Point.Round(iterPoint).Y);
                Thread.Sleep(10);
            }

            // Move the mouse to the final destination.
            SetCursorPos(Point.Round(newPosition).X, Point.Round(newPosition).Y);
        }

        public static void leaveLeftMouse(int x, int y)
        {
            //Cursor.Position = new Point(x, y);
            if (Cursor.Position.X != x || Cursor.Position.Y != y)
            {
                LinearSmoothMove(new Point(x, y), 4);
            }
            //mouse_event(MOUSEEVENTF_MOVE, (uint)x, (uint)y, 0, new UIntPtr(0));
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new UIntPtr(0));
        }

        public static void pressRightMouse(int x, int y)
        {
            if (Cursor.Position.X != x || Cursor.Position.Y != y)
            {
                Cursor.Position = new Point(x, y);
            }
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new UIntPtr(0));
        }

        public static void leaveRighttMouse(int x, int y)
        {
            if (Cursor.Position.X != x || Cursor.Position.Y != y)
            {
                LinearSmoothMove(new Point(x, y), 4);
            }
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new UIntPtr(0));
        }
    }
}
