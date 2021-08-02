using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;

namespace RecordNplay
{
    static class KeysClicker
    {
        public static List<Keys> keysDown = new List<Keys>();
        public static InputSimulator sim = new InputSimulator();
        public volatile static int clickEscape = 0;
        public static void holdKey(byte keyCode,int duration)
        {
            InputManager.Keyboard.KeyDown((Keys)keyCode);
            keysDown.Add((Keys)keyCode);
            new ManualResetEvent(false).WaitOne(duration);
            InputManager.Keyboard.KeyUp((Keys)keyCode);
            keysDown.Remove((Keys)keyCode);
            //Console.WriteLine("Wrote the letter:" + (char)keyCode);
        }
    }
}
