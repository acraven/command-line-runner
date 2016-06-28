namespace CommandLineParser.Tests.Scenarios
{
   using NUnit.Framework;

   public class help
   {
      private StubConsoleWriter _consoleWriter;
      private Runner _testSubject;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         _consoleWriter = new StubConsoleWriter();
         _testSubject = new Runner { ConsoleWriter = _consoleWriter, Name = "CommandLineParser" };
      }

      [TestCase("/?")]
      [TestCase("/help")]
      [TestCase("-help")]
      [TestCase("--help")]
      [TestCase("help")]
      public void display_usage_for_help_related_arguments(string arg)
      {
         _testSubject.Run(new[] { arg });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser");
      }

      // TODO: Missing mandatory arg
      // TODO: Non-null optional args
      // TODO: missing args, default verb, case, verb messages, bool args, named args
   }
}
