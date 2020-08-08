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
        public static List<Keys> keysDown = new List<Keys>();
        public static InputSimulator sim = new InputSimulator();
        public static void holdKey(byte keyCode,int duration)
        {
            
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            InputManager.Keyboard.KeyDown((Keys)keyCode);
            keysDown.Add((Keys)keyCode);
            //sim.Keyboard.KeyDown((VirtualKeyCode)keyCode);
            //sim.Keyboard.Sleep(duration);
            //await Task.Delay(duration);
            new ManualResetEvent(false).WaitOne(duration);
            //while (sw.ElapsedMilliseconds < duration) { }
            InputManager.Keyboard.KeyUp((Keys)keyCode);
            keysDown.Remove((Keys)keyCode);
            //sim.Keyboard.KeyUp((VirtualKeyCode)keyCode);
            //Console.WriteLine("Wrote the letter:" + (char)keyCode);
        }
    }
}
