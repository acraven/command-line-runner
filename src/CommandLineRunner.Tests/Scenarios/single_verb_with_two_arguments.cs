namespace CommandLineParser.Tests.Scenarios
{
   using NUnit.Framework;

   public class single_verb_with_two_arguments
   {
      private StubConsoleWriter _consoleWriter;
      private Runner _testSubject;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         _consoleWriter = new StubConsoleWriter();
         _testSubject = new Runner { ConsoleWriter = _consoleWriter, Name = "CommandLineParser" };
      }

      [Test]
      public void display_usage()
      {
         _testSubject.Register(new Container());
         _testSubject.Run(new[] { "help" });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser",
            "  verb -valueA <string> -valueB <string>");
      }

      [Test]
      public void call_multiple_argument_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-valueA", "myValueForA", "-valueB", "myValueForB" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastValueA, Is.EqualTo("myValueForA"));
         Assert.That(testContainer.LastValueB, Is.EqualTo("myValueForB"));
      }

      [Test]
      public void call_multiple_argument_verb_unordered()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-valueB", "myValueForB", "-valueA", "myValueForA" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastValueA, Is.EqualTo("myValueForA"));
         Assert.That(testContainer.LastValueB, Is.EqualTo("myValueForB"));
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
