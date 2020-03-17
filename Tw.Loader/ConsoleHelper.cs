using System;

namespace TagWriter.Loader
{
  public static class ConsoleHelper
  {
    public static void WriteMessage()
    {
      Console.WriteLine();
    }

    public static void WriteMessage(object message, ConsoleColor color)
    {
      var previousColor = Console.ForegroundColor;

      Console.ForegroundColor = color;
      Console.WriteLine(message);

      Console.ForegroundColor = previousColor;
    }
  }
}
