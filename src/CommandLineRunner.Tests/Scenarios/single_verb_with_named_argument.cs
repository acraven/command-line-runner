namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Shouldly;
   using Xunit;

   public class single_verb_with_named_argument
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public single_verb_with_named_argument()
      {
         _consoleWriter = new StubConsoleWriter();
         _consoleReader = new StubConsoleReader();
         _testSubject = new Runner("CommandLineParser", _consoleWriter, new ArgumentDiscovery(_consoleReader));
      }

      [Fact]
      public async void display_usage()
      {
         _testSubject.Register(new Container());
         await _testSubject.RunAsync(new[] { "--help" });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser [--help|-h]",
            "                         <command> [<args>]",
            "",
            "These are the available commands:",
            "  verb --singleArg|-sa <string>");
      }

      [Fact]
      public async void call_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "--singleArg", "myValueForArg" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastSingleArg.ShouldBe("myValueForArg");
      }

      [Fact]
      public async void call_verb_with_short_name()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-sa", "argValue" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastSingleArg.ShouldBe("argValue");
      }

      [Fact]
      public async void call_verb_without_argument_should_not_call_method()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         testContainer.VerbCalledCount.ShouldBe(0);
      }

      [Fact]
      public async void call_verb_without_argument_should_display_help()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser verb --singleArg|-sa <string>",
            "",
            "Options:",
            "  --singleArg|-sa <string>");
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public string LastSingleArg { get; private set; }

         [Verb]
         public void Verb([Argument(ShortName = "sa")]string singleArg)
         {
            VerbCalledCount++;
            LastSingleArg = singleArg;
         }
      }
   }
}
