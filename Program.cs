using System;
using System.Diagnostics;
using System.Linq;

namespace wifipass
{
    class Program
    {
        static void Main()
        {
            GetCmdOutput("netsh wlan show profiles")
            ?.Split(":")
            ?.Skip(2)
            ?.Select(x => x.Substring(0, x.IndexOf("\r"))
            ?.Trim())
            ?.ToList()
            ?.ForEach(wifi =>
            {
                string password = GetCmdOutput($"netsh wlan show profile name=\"{wifi}\" key=clear")
                ?.Split("\r\n")
                ?.FirstOrDefault(y => y.Contains(" Key Content"))
                ?.Split(":")
                ?.LastOrDefault();
                Console.WriteLine($"{wifi ?? ""} : {password ?? ""}");
            });
        }

        private static string GetCmdOutput(string args)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/c " + args;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            string response = cmd.StandardOutput.ReadToEnd();
            return response;
        }
    }
}
