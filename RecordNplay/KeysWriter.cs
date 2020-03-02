using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;
using WindowsInput;

namespace RecordNplay
{
    static class KeysWriter
    {
        public static InputSimulator sim = new InputSimulator();
        public static void holdKey(byte keyCode,int duration)
        {
            sim.Keyboard.KeyDown((VirtualKeyCode)keyCode);
            //sim.Keyboard.Sleep(duration);
            Thread.Sleep(duration);
            //await waitWithInterrupt(duration);
            sim.Keyboard.KeyUp((VirtualKeyCode)keyCode);
            Console.WriteLine("Wrote the letter:" + (char)keyCode);
        }
        private static async Task waitWithInterrupt(int amountOfSeconds)
        {
            int numOfWaited = 0;
            Console.WriteLine("Hello");
            if (numOfWaited < amountOfSeconds && Form1.running)
            {
                await Task.Delay(1);
                numOfWaited++;
            }
        }

        /*const int PauseBetweenStrokes = 50;
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        public static void HoldKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
                Thread.Sleep(PauseBetweenStrokes);
                totalDuration += PauseBetweenStrokes;
            }
        }*/
    }
}
