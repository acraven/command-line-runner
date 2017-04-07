namespace CommandLineParser.Tests.Scenarios
{
   using Xunit;
   using Shouldly;
   using CommandLineParser.Arguments.Discovery;

   public class single_verb_with_sensitive_argument
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public single_verb_with_sensitive_argument()
      {
         _consoleWriter = new StubConsoleWriter();
         _consoleReader = new StubConsoleReader();
         _testSubject = new Runner("CommandLineParser", _consoleWriter, new ArgumentDiscovery(_consoleReader));
      }

      [Fact]
      public async void display_usage()
      {
         _testSubject.Register(new Container());
         await _testSubject.RunAsync(new[] { "help" });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser",
            "  verb -sensitiveThing <string>");
      }

      [Fact]
      public async void call_multiple_argument_verb_supplied()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-sensitiveThing", "passwordFromArgs" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.SensitiveThing.ShouldBe("passwordFromArgs");
      }

      [Fact]
      public async void call_multiple_argument_verb_missing()
      {
         _consoleReader.Input.Enqueue("passwordFromConsole");

         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.SensitiveThing.ShouldBe("passwordFromConsole");
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public string SensitiveThing { get; private set; }

         [Verb]
         public void Verb([Sensitive]string sensitiveThing)
         {
            VerbCalledCount++;
            SensitiveThing = sensitiveThing;
         }
      }
   }
}
