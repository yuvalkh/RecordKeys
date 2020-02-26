using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;

namespace RecordNplay
{
    class MouseClicker
    {
        static InputSimulator sim = new InputSimulator();
        public static void pressLeftMouse()
        {
            sim.Mouse.LeftButtonDown();
        }
        public static void leaveLeftMouse()
        {

        }
        public static void pressRightMouse()
        {

        }
        public static void leaveRighttMouse()
        {

        }
        /*[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        public static void sendLeftMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 50, 50, 0, new UIntPtr(0));
        }

        public static void sendLeftMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 50, 50, 0, new UIntPtr(0));
        }

        public static void sendRightMouseDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 50, 50, 0, new UIntPtr(0));
        }

        public static void sendRightMouseUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, 50, 50, 0, new UIntPtr(0));
        }*/

    }
}
