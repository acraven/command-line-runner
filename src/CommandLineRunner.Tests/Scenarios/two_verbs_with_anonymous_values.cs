namespace CommandLineParser.Tests.Scenarios
{
   using NUnit.Framework;

   public class two_verbs_with_anonymous_values
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
            "  hash <string>",
            "  random <int32>");
      }

      [Test]
      public void call_first_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "hash", "myValue" });

         Assert.That(testContainer.HashCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastHashValue, Is.EqualTo("myValue"));
      }

      [Test]
      public void call_second_verb()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "random", "23" });

         Assert.That(testContainer.RandomCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastRandomSize, Is.EqualTo(23));
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