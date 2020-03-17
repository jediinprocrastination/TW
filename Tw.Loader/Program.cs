using System;
using System.Diagnostics;
using System.Linq;

namespace TagWriter.Loader
{
  class Program
  {
    private const string INITIAL_COMMAND = "/K DOSKEY validate=MS.DriverValidator.Tool.exe R $*";
    private const string PATH_VARIABLE = "PATH";
    private const string CMD = "cmd";

    private static int Main(string[] args)
    {
      try
      {
        return Run(args);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return (int)TWExitCodes.NOK;
      }
    }

    private static int Run(string[] args)
    {
      if (args.Any())
      {
        #if DEBUG
        ConsoleHelper.WriteMessage("DEBUG: press any key to continue...\n", ConsoleColor.Gray);
        Console.ReadKey();
        #endif

        var result = Start(args);

        #if DEBUG
        ConsoleHelper.WriteMessage("DEBUG: press any key to continue...\n", ConsoleColor.Gray);
        Console.ReadKey();
        #endif

        return result;
      }
      else
      {
        SetupEnvironment();
      }

      return (int)TWExitCodes.OK;
    }

    private static void SetupEnvironment()
    {
      var path = Environment.GetEnvironmentVariable(PATH_VARIABLE);

      if (path == null)
        throw new ApplicationException("Something went wrong :`(");

      var cd = Environment.CurrentDirectory;

      if (!path.Contains(cd)) path = $"{cd};{path}";

      var startInfo = new ProcessStartInfo(CMD, INITIAL_COMMAND);

      startInfo.EnvironmentVariables[PATH_VARIABLE] = path;
      startInfo.UseShellExecute = false;

      Process.Start(startInfo);
    }

    private static int Start(string[] args)
    {
      try 
      {
        StartCore(args);
        return (int)TWExitCodes.OK;
      }
      catch (TWApplicationException e)
      {
        return (int)e.Code;
      }
    }

    private static void StartCore(string[] args)
    {
      var arguments = args.Where(x => !string.Equals(x, "R")).ToArray();
      var parser = new ArgumentParser(arguments);

      if (parser.SilentMode)
      {
        return;
      }
    }
  }

  internal enum TWExitCodes
  {
    OK = 0,
    NOK = -1
  }

  internal class TWApplicationException : ApplicationException
  {
    public TWApplicationException(TWExitCodes code)
    {
      Code = code;
    }

    public TWExitCodes Code { get; }
  }
}
