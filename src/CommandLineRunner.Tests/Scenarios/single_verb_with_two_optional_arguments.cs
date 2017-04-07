namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Shouldly;
   using Xunit;

   public class single_verb_with_two_optional_arguments
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public single_verb_with_two_optional_arguments()
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
            "  verb [-stringValue <string>] [-intValue <int32>]");
      }

      [Fact]
      public async void call_optional_argument_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-stringValue", "hello", "-intValue", "23" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastStringValue.ShouldBe("hello");
         testContainer.LastIntValue.ShouldBe(23);
      }

      [Fact]
      public async void call_optional_argument_verb_without_arguments()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastStringValue.ShouldBeNull();
         testContainer.LastIntValue.ShouldBeNull();
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public string LastStringValue { get; private set; }

         public int? LastIntValue { get; private set; }

         [Verb]
         public void Verb(string stringValue = null, int? intValue = null)
         {
            VerbCalledCount++;
            LastStringValue = stringValue;
            LastIntValue = intValue;
         }
      }
   }
}
