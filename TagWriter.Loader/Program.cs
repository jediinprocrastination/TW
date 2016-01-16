using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace TagWriter.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.Unicode;

            var pathVariable = Environment.GetEnvironmentVariable("PATH");
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var applicationPath = Path.GetDirectoryName(assemblyPath);

            var info = new ProcessStartInfo();

            info.EnvironmentVariables["PATH"] = string.Format("{0};{1}", pathVariable, applicationPath);
            info.UseShellExecute = false;
            info.FileName = "cmd.exe";

            Process.Start(info);
        }
    }
}
