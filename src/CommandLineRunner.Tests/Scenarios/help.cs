namespace CommandLineParser.Tests.Scenarios
{
   using Xunit;
   using CommandLineParser.Arguments.Discovery;

   public class help
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public help()
      {
         _consoleWriter = new StubConsoleWriter();
         _consoleReader = new StubConsoleReader();
         _testSubject = new Runner("CommandLineParser", _consoleWriter, new ArgumentDiscovery(_consoleReader));
      }

      [Theory]
      [InlineData("/?")]
      [InlineData("/help")]
      [InlineData("-help")]
      [InlineData("--help")]
      [InlineData("help")]
      public async void display_usage_for_help_related_arguments(string arg)
      {
         await _testSubject.RunAsync(new[] { arg });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser");
      }

      // TODO: Missing mandatory arg
      // TODO: Non-null optional args
      // TODO: missing args, default verb, case, verb messages, bool args, named args
   }
}
