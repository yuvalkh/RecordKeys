﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using WindowsInput.Native;

namespace RecordNplay
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    class globalKeyboardHook
    {
        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

        public Form1 form1;

        public List<Keys> keysHolding = new List<Keys>();

        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;
        #endregion

        #region Instance Variables
        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;
        #endregion

        #region Events
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Form1.running)
            {
                if (Form1.recording)
                {
                    PressedKeyInfo keyInfo = getLastOccurance(Form1.writingChars, (byte)e.KeyValue);
                    if (keyInfo != null)
                    {
                        keyInfo.duration = Form1.sw.ElapsedMilliseconds - keyInfo.startTime;
                    }
                }
                else//To start the macro
                {
                    if(form1.comboBox1.Text.Equals(e.KeyCode.ToString()))
                    {
                        new Thread(() =>
                        {
                            form1.runMacro();
                        }).Start(); 
                    }
                }
            }
            else
            {
                if((byte)e.KeyValue == 27)//it means we need to stop the macro
                {
                    releaseAllKeys();
                    Form1.running = false;
                }
            }
            Console.WriteLine("Up\t" + e.KeyCode.ToString());
            //e.Handled = true;
        }

        private void releaseAllKeys()
        {
            for (int i = 0; i < keysHolding.Count; i++)
            {
                KeysWriter.sim.Keyboard.KeyUp((VirtualKeyCode)keysHolding[i]);
            }
        }

        private PressedKeyInfo getLastOccurance(List<PressedInput> inputs, byte keyCode)
        {
            for (int i = inputs.Count - 1; i >= 0; i--)
            {
                if (inputs[i] is PressedKeyInfo && ((PressedKeyInfo)inputs[i]).keyCode == keyCode)
                {
                    return ((PressedKeyInfo)inputs[i]);
                }
            }
            return null;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Form1.running && Form1.recording)
            {
               Form1.writingChars.Add(new PressedKeyInfo((byte)e.KeyValue, 0, Form1.sw.ElapsedMilliseconds));
            }
            Console.WriteLine("Down\t" + e.KeyCode.ToString());
            //e.Handled = true;
        }
        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
        /// </summary>
        public globalKeyboardHook(Form1 form)
        {
            form1 = form;
            KeyDown += new KeyEventHandler(gkh_KeyDown);
            KeyUp += new KeyEventHandler(gkh_KeyUp);
            //hook();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~globalKeyboardHook()
        {
            unhook();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs the global hook
        /// </summary>
        private static keyboardHookProc callbackDelegate;

        public void hook()
        {
            if (callbackDelegate != null) return; //throw new InvalidOperationException("Can't hook more than once");
            IntPtr hInstance = LoadLibrary("User32");
            callbackDelegate = new keyboardHookProc(hookProc);
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hInstance, 0);
            if (hhook == IntPtr.Zero) throw new Win32Exception();
        }

        public void unhook()
        {
            if (callbackDelegate == null) return;
            bool ok = UnhookWindowsHookEx(hhook);
            if (!ok) throw new Win32Exception();
            callbackDelegate = null;
        }
        /*public void hook()
    {
        IntPtr hInstance = LoadLibrary("User32");
        hhook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hInstance, 0);
    }

    /// <summary>
    /// Uninstalls the global hook
    /// </summary>
    public void unhook()
    {
        UnhookWindowsHookEx(hhook);
    }*/

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                KeyEventArgs kea = new KeyEventArgs(key);
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                {
                    if (!keysHolding.Contains(kea.KeyCode))
                    {
                        keysHolding.Add(kea.KeyCode);
                        gkh_KeyDown(this, kea);
                    }
                }
                else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                {
                    keysHolding.Remove(kea.KeyCode);
                    gkh_KeyUp(this, kea);
                }
                if (kea.Handled)
                    return 1;
                //}
            }
            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }
        #endregion

        #region DLL imports
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion
    }
}
