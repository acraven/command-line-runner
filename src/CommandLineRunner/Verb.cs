namespace CommandLineRunner
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using System.Threading.Tasks;
   using CommandLineRunner.Arguments;
   using CommandLineRunner.Arguments.Discovery;

   public class Verb
   {
      private readonly IArgument[] _arguments;
      private readonly MethodInfo _verbMethod;
      private readonly object _container;

      public string Name { get; }

      public Verb(MethodInfo verbMethod, object container, IDiscoverArguments argumentDiscovery)
      {
         Name = verbMethod.Name.ToCamelCase();
         _arguments = verbMethod.GetParameters().Select(argumentDiscovery.Discover).ToArray();
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

      public async Task RunAsync(params object[] args)
      {
         var task = _verbMethod.Invoke(_container, args) as Task;
         if (task != null)
         {
            await task;
         }
      }

      public void WriteUsage(IWriteToConsole consoleWriter)
      {
         var argNames = string.Join(" ", _arguments.Select(c => c.ToString()));

         consoleWriter.Write("  {0} {1}", Name, argNames);
      }
   }
}