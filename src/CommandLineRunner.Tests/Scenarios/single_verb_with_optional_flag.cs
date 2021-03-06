﻿namespace CommandLineRunner.Tests.Scenarios
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
         await _testSubject.RunAsync(new[] { "--help" });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser [--help|-h]",
            "                         <command> [<args>]",
            "",
            "These are the available commands:",
            "  verb [--flag|-f]");
      }

      [Fact]
      public async void call_verb_with_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "--flag" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastFlag.ShouldBe(true);
      }

      [Fact]
      public async void call_verb_with_flag_argument_short_name()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb", "-f" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastFlag.ShouldBe(true);
      }

      [Fact]
      public async void call_verb_without_flag_argument()
      {
         var testContainer = new Container();
         _testSubject.Register(testContainer);
         await _testSubject.RunAsync(new[] { "verb" });

         testContainer.VerbCalledCount.ShouldBe(1);
         testContainer.LastFlag.ShouldBe(false);
      }

      public class Container
      {
         public int VerbCalledCount { get; private set; }

         public bool LastFlag { get; private set; }

         [Verb]
         public void Verb([Argument(ShortName = "f")]bool flag)
         {
            VerbCalledCount++;
            LastFlag = flag;
         }
      }
   }
}
