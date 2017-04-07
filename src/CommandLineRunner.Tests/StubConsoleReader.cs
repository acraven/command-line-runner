namespace CommandLineRunner.Tests
{
   using System.Collections.Generic;

   public class StubConsoleReader : IReadFromConsole
   {
      public Queue<string> Input { get; } = new Queue<string>();

      public string ReadSensitive(string prompt)
      {
         return Input.Dequeue();
      }
   }
}