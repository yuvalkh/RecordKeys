using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordNplay
{
    public partial class Form1 : Form
    {
        public static int totalTime = 0;
        public static Stopwatch sw = Stopwatch.StartNew();
        globalKeyboardHook gkh = new globalKeyboardHook();
        bool clicked = false;
        public static List<PressedInput> writingChars = new List<PressedInput>();

        public Form1()
        {
            InitializeComponent();
            sw.Start();
            MouseHook.Start();
            //MouseHook.stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            totalTime++;
            //get coordinates
            label2.Text = Cursor.Position.X.ToString();
            label4.Text = Cursor.Position.Y.ToString();
            //move to coordinates
            //Cursor.Position = new Point(10, 10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gkh.hook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gkh.unhook();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            for (int i = 0; i < writingChars.Count; i++)
            {
                if(i == 0)
                {
                    if(writingChars[i] is PressedKeyInfo)
                    {
                        Thread.Sleep((int)((PressedKeyInfo)writingChars[i]).startTime);
                    }
                    //KeysWriter.sim.Keyboard.Sleep((int)writingChars[i].startTime);
                }
                else
                {
                    if (writingChars[i] is PressedKeyInfo)
                    {
                        Thread.Sleep((int)(((PressedKeyInfo)writingChars[i]).startTime - ((PressedKeyInfo)writingChars[i - 1]).startTime));
                    }
                    else
                    {

                    }
                        //KeysWriter.sim.Keyboard.Sleep((int)(writingChars[i].startTime - writingChars[i - 1].startTime));
                }
                if(writingChars[i] is PressedKeyInfo)
                {
                    
                    byte tempKeyCode = ((PressedKeyInfo)writingChars[i]).keyCode;
                    int tempDuration = (int)((PressedKeyInfo)writingChars[i]).duration;
                    new Task(() =>
                    {
                        //KeysWriter.holdKey(writingChars[i].keyCode, (int)writingChars[i].duration);
                        KeysWriter.holdKey(tempKeyCode, tempDuration);
                    }).Start();
                }
                else//if it's a mouse
                {

                }
                
                //Console.WriteLine((char)writingChars[i].keyCode);
            }
        }
    }
}
