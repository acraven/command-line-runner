using System.Collections.Generic;
using NUnit.Framework;

namespace CommandLineParser.Tests
{
   public class StubConsoleWriter : IWriteToConsole
   {
      public List<string> Messages { get; } = new List<string>();

      public void Write(string format, params object[] args)
      {
         Messages.Add(string.Format(format, args));
      }

      public void AssertWrittenMessages(params string[] expectedMessages)
      {
         CollectionAssert.AreEqual(expectedMessages, Messages.ToArray());
      }
   }
}
