namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Shouldly;
   using Xunit;

   public class two_verbs_with_anonymous_values
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public two_verbs_with_anonymous_values()
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
            "  hash <string>",
            "  random <int32>");
      }

      [Fact]
      public async void call_first_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "hash", "myValue" });

         testContainer.HashCalledCount.ShouldBe(1);
         testContainer.LastHashValue.ShouldBe("myValue");
      }

      [Fact]
      public async void call_second_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "random", "23" });

         testContainer.RandomCalledCount.ShouldBe(1);
         testContainer.LastRandomSize.ShouldBe(23);
      }

      public class Container
      {
         public int HashCalledCount { get; private set; }

         public int RandomCalledCount { get; private set; }

         public string LastHashValue { get; private set; }

         public int LastRandomSize { get; private set; }

         [Verb]
         public void Hash([Anonymous]string value)
         {
            HashCalledCount++;
            LastHashValue = value;
         }

         [Verb]
         public void Random([Anonymous]int size)
         {
            RandomCalledCount++;
            LastRandomSize = size;
         }
      }
   }

}