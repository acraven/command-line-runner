﻿namespace CommandLineRunner.Tests.Scenarios
{
   using CommandLineRunner.Arguments.Discovery;
   using Xunit;

   public class help
   {
      private readonly StubConsoleWriter _consoleWriter;
      private readonly StubConsoleReader _consoleReader;
      private readonly Runner _testSubject;

      public help()
      {
         _consoleWriter = new StubConsoleWriter();
         _consoleReader = new StubConsoleReader();
         _testSubject = new Runner("CommandLineParser", _consoleWriter, new ArgumentDiscovery(_consoleReader));
         var testContainer = new Container();
         _testSubject.Register(testContainer);
      }

      [Theory]
      [InlineData("/?")]
      [InlineData("/help")]
      [InlineData("-help")]
      [InlineData("-h")]
      [InlineData("--help")]
      [InlineData("--h")]
      [InlineData("help")]
      public async void display_usage_for_help_related_arguments(string arg)
      {
         await _testSubject.RunAsync(new[] { arg });

         _consoleWriter.AssertWrittenOutput(
            "USAGE: CommandLineParser [--help|-h]",
            "                         <command> [<args>]",
            "",
            "These are the available commands:",
            "  verb --singleArg <string>");
      }

      public class Container
      {
         [Verb]
         public void Verb(string singleArg)
         {
         }
      }

      // TODO: Missing mandatory arg
      // TODO: Non-null optional args
      // TODO: missing args, default verb, case, verb messages, bool args, named args
   }
}
