using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Culldy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No Args");
                Environment.Exit(1);
            }
            switch (args[0])
            {
                case "launch":
                    LaunchGame();
                    break;
                default:
                    Console.WriteLine("Unknown Args");
                    Console.WriteLine(args[0]);
                    break;
            }
        }
        static void LaunchGame()
        {
            string winver = (string)Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ProductName");
            if (winver.IndexOf("Windows 10") > -1 || winver.IndexOf("Windows 11") > -1)
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.RedirectStandardInput = true;

                    p.Start();

                    p.StandardInput.WriteLine("powershell /c start \"shell:appsfolder\\$(Get-AppxPackage -Name Microsoft.MinecraftUWP | select -ExpandProperty packagefamilyname)!app\" && exit");
                    p.StandardInput.AutoFlush = true;
                    if (!(p.StandardError.ReadLine() == null))
                    {
                        Console.WriteLine("Start Failed");
                        Environment.Exit(1);
                    }

                    p.WaitForExit();
                    p.Close();
                }
            }
            else
            {
                Console.WriteLine("Not Support System");
                Environment.Exit(1);
            }
        }
    }
}
