namespace CommandLineParser.Tests.Scenarios
{
   using NUnit.Framework;

   public class single_verb_with_optional_flag
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
            "  verb [-flagA]");
      }

      [Test]
      public void call_verb_with_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-flagA" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastFlagA, Is.True);
      }

      [Test]
      public void call_verb_without_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastFlagA, Is.False);
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
