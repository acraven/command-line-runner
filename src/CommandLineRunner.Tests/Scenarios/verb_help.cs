namespace CommandLineRunner.Tests.Scenarios
{
   using System.ComponentModel;
   using CommandLineRunner.Arguments.Discovery;
   using Xunit;

   public class verb_help
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public verb_help()
      {
         _consoleWriter = new StubConsoleWriter();
         _consoleReader = new StubConsoleReader();
         _testSubject = new Runner("CommandLineParser", _consoleWriter, new ArgumentDiscovery(_consoleReader));
         var testContainer = new Container();
         _testSubject.Register(testContainer);
      }

      [Theory]
      [InlineData("-h")]
      [InlineData("--help")]
      public async void display_verb_help(string arg)
      {
         await _testSubject.RunAsync(new[] { arg, "verb" });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser verb --singleArg|-sa <string>",
            "",
            "Description of verb",
            "",
            "Options:",
            "  --singleArg|-sa <string> Description of argument",
            "",
            "First line of verb help",
            "Second line of verb help");
      }

      public class Container
      {
         [Verb(Description = "Description of verb")]
         public void Verb(
            [Argument(ShortName = "sa", Description = "Description of argument")]string singleArg)
         {
         }

         public void VerbHelp(IWriteToConsole consolerWriter)
         {
            consolerWriter.WriteLine("First line of verb help");
            consolerWriter.WriteLine("Second line of verb help");
         }
      }
   }
}
