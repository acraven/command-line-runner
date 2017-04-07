namespace CommandLineParser.Tests
{
   using System.Collections.Generic;
   using Shouldly;

   public class StubConsoleWriter : IWriteToConsole
   {
      public List<string> Output { get; } = new List<string>();

      public void Write(string format, params object[] args)
      {
         Output.Add(string.Format(format, args));
      }

      public void AssertWrittenOutput(params string[] expectedMessages)
      {
         expectedMessages.ShouldBe(Output);
      }
   }
}
