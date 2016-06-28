namespace CommandLineParser
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using CommandLineParser.Arguments;
   using CommandLineParser.Arguments.Discovery;

   public class Verb
   {
      private readonly string _name;
      private readonly IArgument[] _arguments;
      private readonly MethodInfo _verbMethod;
      private readonly object _container;

      public IDiscoverArguments ArgumentDiscovery { get; set; } = new ArgumentDiscovery();

      public string Name => _name;

      public Verb(MethodInfo verbMethod, object container)
      {
         _name = verbMethod.Name.ToCamelCase();
         _arguments = verbMethod.GetParameters().Select(p => ArgumentDiscovery.Discover(p)).ToArray();
         _verbMethod = verbMethod;
         _container = container;
      }

      public object[] ParseArguments(LinkedList<string> argsToParse)
      {
         var parsedArgs = new List<Tuple<IArgument, object>>();

         foreach (var argument in _arguments)
         {
            argument.Parse(argsToParse, parsedArgs);
         }

         return parsedArgs.Select(c => c.Item2).ToArray();
      }

      public void Run(params object[] args)
      {
         _verbMethod.Invoke(_container, args);
      }

      public void WriteUsage(IWriteToConsole consoleWriter)
      {
         var argNames = string.Join(" ", _arguments.Select(c => c.ToString()));

         consoleWriter.Write("  {0} {1}", _name, argNames);
      }
   }
}