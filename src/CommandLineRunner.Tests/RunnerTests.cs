using NUnit.Framework;

namespace CommandLineParser.Tests
{
   public class RunnerTests
   {
      private StubConsoleWriter _consoleWriter;
      private Runner _testSubject;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         _consoleWriter = new StubConsoleWriter();
         _testSubject = new Runner { ConsoleWriter = _consoleWriter, Name = "CommandLineParser" };
      }

      [TestCase("/?")]
      [TestCase("/help")]
      [TestCase("-help")]
      [TestCase("--help")]
      [TestCase("help")]
      public void display_usage_for_help_related_arguments(string arg)
      {
         _testSubject.Run(new[] { arg });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser");
      }

      [Test]
      public void display_usage_including_registered_verbs()
      {
         _testSubject.Register(new ContainerWithTwoSimpleVerbs());
         _testSubject.Run(new[] { "help" });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser",
            "  hash <value:string>",
            "  random <size:int32>");
      }

      [Test]
      public void call_first_verb()
      {
         var testContainer = new ContainerWithTwoSimpleVerbs();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "hash", "myValue" });

         Assert.That(testContainer.HashCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastHashValue, Is.EqualTo("myValue"));
      }

      [Test]
      public void call_second_verb()
      {
         var testContainer = new ContainerWithTwoSimpleVerbs();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "random", "23" });

         Assert.That(testContainer.RandomCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastRandomSize, Is.EqualTo(23));
      }

      [Test]
      public void display_usage_for_multiple_argument_verb()
      {
         _testSubject.Register(new ContainerWithTwoArgumentVerb());
         _testSubject.Run(new[] { "help" });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser",
            "  verb -valueA <string> -valueB <string>");
      }

      [Test]
      public void call_multiple_argument_verb()
      {
         var testContainer = new ContainerWithTwoArgumentVerb();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-valueA", "myValueForA", "-valueB", "myValueForB" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastValueA, Is.EqualTo("myValueForA"));
         Assert.That(testContainer.LastValueB, Is.EqualTo("myValueForB"));
      }

      [Test]
      public void call_multiple_argument_verb_unordered()
      {
         var testContainer = new ContainerWithTwoArgumentVerb();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-valueB", "myValueForB", "-valueA", "myValueForA" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastValueA, Is.EqualTo("myValueForA"));
         Assert.That(testContainer.LastValueB, Is.EqualTo("myValueForB"));
      }

      [Test]
      public void display_usage_for_optional_argument_verb()
      {
         _testSubject.Register(new ContainerWithOptionalTwoArgumentVerb());
         _testSubject.Run(new[] { "help" });

         _consoleWriter.AssertWrittenMessages(
            "USAGE: CommandLineParser",
            "  verb [-stringValue <string>] [-intValue <int32>]");
      }

      [Test]
      public void call_optional_argument_verb()
      {
         var testContainer = new ContainerWithOptionalTwoArgumentVerb();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb", "-stringValue", "hello", "-intValue", "23" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastStringValue, Is.EqualTo("hello"));
         Assert.That(testContainer.LastIntValue, Is.EqualTo(23));
      }

      [Test]
      public void call_optional_argument_verb_without_arguments()
      {
         var testContainer = new ContainerWithOptionalTwoArgumentVerb();
         _testSubject.Register(testContainer);
         _testSubject.Run(new[] { "verb" });

         Assert.That(testContainer.VerbCalledCount, Is.EqualTo(1));
         Assert.That(testContainer.LastStringValue, Is.Null);
         Assert.That(testContainer.LastIntValue, Is.Null);
      }

      // TODO: Missing mandatory arg
      // TODO: Non-null optional args
      // TODO: missing args, default verb, case, verb messages, bool args, named args
   }
}
