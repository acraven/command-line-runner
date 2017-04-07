namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Shouldly;
   using Xunit;

   public class single_verb_with_optional_flag
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public single_verb_with_optional_flag()
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
            "  verb [-flagA]");
      }

      [Fact]
      public async void call_verb_with_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-flagA" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastFlagA.ShouldBe(true);
      }

      [Fact]
      public async void call_verb_without_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastFlagA.ShouldBe(false);
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public bool LastFlagA { get; private set; }

         [Verb]
         public void Verb(bool flagA)
         {
            VerbCalledCount++;
            LastFlagA = flagA;
         }
      }
   }
}
