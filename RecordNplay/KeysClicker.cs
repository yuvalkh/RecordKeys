﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        const uint VM_KEYDOWN = 0x0100;
        const uint VM_KEYUP = 0x0101;
        const uint VM_CHAR = (0x0102); //The character being pressed
        const int VK_TAB = 0x09;
        const int VK_ENTER = 0x0D;
        const int VK_UP = 0x26;
        const int VK_DOWN = 0x28;
        const int VK_RIGHT = 0x27;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void processHoldKey(IntPtr hWnd,byte keyCode, int duration)
        {
            /*PostMessage(hWnd, VM_KEYDOWN, (IntPtr)65, IntPtr.Zero);
			PostMessage(hWnd, VM_CHAR, (IntPtr)65, IntPtr.Zero);
			//new ManualResetEvent(false).WaitOne(duration);
			PostMessage(hWnd, VM_KEYUP, (IntPtr)65, IntPtr.Zero);*/
            

			PostMessage(hWnd, VM_KEYDOWN, (IntPtr)VKeys.KEY_A, GetLParam(1, VKeys.KEY_A, 0, 0, 0, 0));
			PostMessage(hWnd, VM_KEYUP, (IntPtr)VKeys.KEY_A, GetLParam(1, VKeys.KEY_A, 0, 0, 0, 0));
        }

        ////////////////////////////////////////
        ////////////////////////////////////////
        ////////////////////////////////////////
        ////////////////////////////////////////
        ////////////////////////////////////////
        ////////////////////////////////////////
        //////////////////////////////////////
        ////////////////
        ///
        
        
        
        
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		private const uint MAPVK_VK_TO_VSC_EX = 0x04;

		private static uint GetScanCode(VKeys key)
        {
            return MapVirtualKey((uint)key, MAPVK_VK_TO_VSC_EX);
        }

        private static IntPtr GetLParam(Int16 repeatCount, VKeys key, byte extended, byte contextCode, byte previousState,
            byte transitionState)
        {
            var lParam = (uint)repeatCount;
            //uint scanCode = MapVirtualKey((uint)key, MAPVK_VK_TO_CHAR);
            uint scanCode = GetScanCode(key);
            lParam += scanCode * 0x10000;
            lParam += (uint)((extended) * 0x1000000);
            lParam += (uint)((contextCode * 2) * 0x10000000);
            lParam += (uint)((previousState * 4) * 0x10000000);
            lParam += (uint)((transitionState * 8) * 0x10000000);
            return (IntPtr)lParam;
        }
		[Serializable]
		public enum VKeys
		{
			KEY_0 = 0x30, //0 key 
			KEY_1 = 0x31, //1 key 
			KEY_2 = 0x32, //2 key 
			KEY_3 = 0x33, //3 key 
			KEY_4 = 0x34, //4 key 
			KEY_5 = 0x35, //5 key 
			KEY_6 = 0x36, //6 key 
			KEY_7 = 0x37, //7 key 
			KEY_8 = 0x38, //8 key 
			KEY_9 = 0x39, //9 key
			KEY_MINUS = 0xBD, // - key
			KEY_PLUS = 0xBB, // + key
			KEY_A = 0x41, //A key 
			KEY_B = 0x42, //B key 
			KEY_C = 0x43, //C key 
			KEY_D = 0x44, //D key 
			KEY_E = 0x45, //E key 
			KEY_F = 0x46, //F key 
			KEY_G = 0x47, //G key 
			KEY_H = 0x48, //H key 
			KEY_I = 0x49, //I key 
			KEY_J = 0x4A, //J key 
			KEY_K = 0x4B, //K key 
			KEY_L = 0x4C, //L key 
			KEY_M = 0x4D, //M key 
			KEY_N = 0x4E, //N key 
			KEY_O = 0x4F, //O key 
			KEY_P = 0x50, //P key 
			KEY_Q = 0x51, //Q key 
			KEY_R = 0x52, //R key 
			KEY_S = 0x53, //S key 
			KEY_T = 0x54, //T key 
			KEY_U = 0x55, //U key 
			KEY_V = 0x56, //V key 
			KEY_W = 0x57, //W key 
			KEY_X = 0x58, //X key 
			KEY_Y = 0x59, //Y key 
			KEY_Z = 0x5A, //Z key 
			KEY_LBUTTON = 0x01, //Left mouse button 
			KEY_RBUTTON = 0x02, //Right mouse button 
			KEY_CANCEL = 0x03, //Control-break processing 
			KEY_MBUTTON = 0x04, //Middle mouse button (three-button mouse) 
			KEY_BACK = 0x08, //BACKSPACE key 
			KEY_TAB = 0x09, //TAB key 
			KEY_CLEAR = 0x0C, //CLEAR key 
			KEY_RETURN = 0x0D, //ENTER key 
			KEY_SHIFT = 0x10, //SHIFT key 
			KEY_CONTROL = 0x11, //CTRL key 
			KEY_MENU = 0x12, //ALT key 
			KEY_PAUSE = 0x13, //PAUSE key 
			KEY_CAPITAL = 0x14, //CAPS LOCK key 
			KEY_ESCAPE = 0x1B, //ESC key 
			KEY_SPACE = 0x20, //SPACEBAR 
			KEY_PRIOR = 0x21, //PAGE UP key 
			KEY_NEXT = 0x22, //PAGE DOWN key 
			KEY_END = 0x23, //END key 
			KEY_HOME = 0x24, //HOME key 
			KEY_LEFT = 0x25, //LEFT ARROW key 
			KEY_UP = 0x26, //UP ARROW key 
			KEY_RIGHT = 0x27, //RIGHT ARROW key 
			KEY_DOWN = 0x28, //DOWN ARROW key 
			KEY_SELECT = 0x29, //SELECT key 
			KEY_PRINT = 0x2A, //PRINT key 
			KEY_EXECUTE = 0x2B, //EXECUTE key 
			KEY_SNAPSHOT = 0x2C, //PRINT SCREEN key 
			KEY_INSERT = 0x2D, //INS key 
			KEY_DELETE = 0x2E, //DEL key 
			KEY_HELP = 0x2F, //HELP key 
			KEY_NUMPAD0 = 0x60, //Numeric keypad 0 key 
			KEY_NUMPAD1 = 0x61, //Numeric keypad 1 key 
			KEY_NUMPAD2 = 0x62, //Numeric keypad 2 key 
			KEY_NUMPAD3 = 0x63, //Numeric keypad 3 key 
			KEY_NUMPAD4 = 0x64, //Numeric keypad 4 key 
			KEY_NUMPAD5 = 0x65, //Numeric keypad 5 key 
			KEY_NUMPAD6 = 0x66, //Numeric keypad 6 key 
			KEY_NUMPAD7 = 0x67, //Numeric keypad 7 key 
			KEY_NUMPAD8 = 0x68, //Numeric keypad 8 key 
			KEY_NUMPAD9 = 0x69, //Numeric keypad 9 key 
			KEY_SEPARATOR = 0x6C, //Separator key 
			KEY_SUBTRACT = 0x6D, //Subtract key 
			KEY_DECIMAL = 0x6E, //Decimal key 
			KEY_DIVIDE = 0x6F, //Divide key 
			KEY_F1 = 0x70, //F1 key 
			KEY_F2 = 0x71, //F2 key 
			KEY_F3 = 0x72, //F3 key 
			KEY_F4 = 0x73, //F4 key 
			KEY_F5 = 0x74, //F5 key 
			KEY_F6 = 0x75, //F6 key 
			KEY_F7 = 0x76, //F7 key 
			KEY_F8 = 0x77, //F8 key 
			KEY_F9 = 0x78, //F9 key 
			KEY_F10 = 0x79, //F10 key 
			KEY_F11 = 0x7A, //F11 key 
			KEY_F12 = 0x7B, //F12 key 
			KEY_SCROLL = 0x91, //SCROLL LOCK key 
			KEY_LSHIFT = 0xA0, //Left SHIFT key 
			KEY_RSHIFT = 0xA1, //Right SHIFT key 
			KEY_LCONTROL = 0xA2, //Left CONTROL key 
			KEY_RCONTROL = 0xA3, //Right CONTROL key 
			KEY_LMENU = 0xA4, //Left MENU key 
			KEY_RMENU = 0xA5, //Right MENU key 
			KEY_COMMA = 0xBC, //, key
			KEY_PERIOD = 0xBE, //. key
			KEY_PLAY = 0xFA, //Play key 
			KEY_ZOOM = 0xFB, //Zoom key 
			NULL = 0x0,
		}
	}
}
