namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Shouldly;
   using Xunit;

   public class single_verb_with_two_arguments
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public single_verb_with_two_arguments()
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
            "  verb -valueA <string> -valueB <string>");
      }

      [Fact]
      public async void call_multiple_argument_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-valueA", "myValueForA", "-valueB", "myValueForB" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastValueA.ShouldBe("myValueForA");
         testContainer.LastValueB.ShouldBe("myValueForB");
      }

      [Fact]
      public async void call_multiple_argument_verb_unordered()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-valueB", "myValueForB", "-valueA", "myValueForA" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastValueA.ShouldBe("myValueForA");
         testContainer.LastValueB.ShouldBe("myValueForB");
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public string LastValueA { get; private set; }

         public string LastValueB { get; private set; }

         [Verb]
         public void Verb(string valueA, string valueB)
         {
            VerbCalledCount++;
            LastValueA = valueA;
            LastValueB = valueB;
         }
      }
   }
}
