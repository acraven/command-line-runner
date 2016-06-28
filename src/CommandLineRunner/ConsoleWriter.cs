namespace CommandLineParser
{
   using System;

   public class ConsoleWriter : IWriteToConsole
   {
      public void Write(string format, params object[] args)
      {
         Console.WriteLine(format, args);
      }
   }
}