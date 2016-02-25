using System;

namespace CommandLineParser
{
   public class ConsoleWriter : IWriteToConsole
   {
      public void Write(string message)
      {
         Console.WriteLine(message);
      }

      public void Write(string format, params object[] args)
      {
         Console.WriteLine(format, args);
      }
   }
}