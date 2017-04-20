namespace CommandLineRunner.Tests
{
   using System;
   using System.Text;
   using Shouldly;

   public class StubConsoleWriter : IWriteToConsole
   {
      public StringBuilder Output { get; } = new StringBuilder();

      public void Write(string format, params object[] args)
      {
         Output.Append(string.Format(format, args));
      }

      public void WriteLine(string format, params object[] args)
      {
         Output.AppendLine(string.Format(format, args));
      }

      public void AssertWrittenOutput(params string[] expectedMessages)
      {
         var expected = string.Join(Environment.NewLine, expectedMessages);
         expected += Environment.NewLine;

         Output.ToString().ShouldBe(expected);
      }
   }
}
