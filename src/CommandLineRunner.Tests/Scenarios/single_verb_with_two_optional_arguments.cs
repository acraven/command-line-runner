namespace CommandLineParser.Tests.Scenarios
{
   using NUnit.Framework;

   public class single_verb_with_two_optional_arguments
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
            "  verb [-stringValue <string>] [-intValue <int32>]");
      }

      [Test]
      public void call_optional_argument_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-stringValue", "hello", "-intValue", "23" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastStringValue, Is.EqualTo("hello"));
         Assert.That(testContainer.LastIntValue, Is.EqualTo(23));
      }

      [Test]
      public void call_optional_argument_verb_without_arguments()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastStringValue, Is.Null);
         Assert.That(testContainer.LastIntValue, Is.Null);
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
