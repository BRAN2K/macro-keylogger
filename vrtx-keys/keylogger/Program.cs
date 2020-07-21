using System;
using System.IO;
using System.Windows.Forms;

namespace vrtx_keys {
    class Program {
        private static IntPtr hook = IntPtr.Zero;

        static void Main(string[] args) {
            hook = KeyboardListener.SetHook(KeyboardListener.llkProcedure);
            Application.Run();
            //UnhookWindowsHookEx(hook);
        }
    }
}