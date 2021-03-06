﻿namespace CommandLineRunner
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using System.Text;
   using System.Threading.Tasks;
   using CommandLineRunner.Arguments;
   using CommandLineRunner.Arguments.Discovery;

   public class Verb
   {
      private readonly IArgument[] _arguments;
      private readonly MethodInfo _verbMethod;
      private readonly MethodInfo _helpMethod;
      private readonly object _container;

      public string Name { get; }

      public string Description { get; }

      public Verb(string description, MethodInfo verbMethod, object container, IDiscoverArguments argumentDiscovery)
      {
         Name = verbMethod.Name.ToCamelCase();
         Description = description;
         _arguments = verbMethod.GetParameters().Select(argumentDiscovery.Discover).ToArray();
         _verbMethod = verbMethod;
         _container = container;

         var helpMethod = container.GetType().GetMethod(verbMethod.Name + "Help", BindingFlags.Public | BindingFlags.Instance);

         var parameters = helpMethod?.GetParameters();
         if (parameters?.Length == 1 && parameters[0].ParameterType == typeof(IWriteToConsole))
         {
            _helpMethod = helpMethod;
         }
      }

      public ParseResponse ParseArguments(LinkedList<string> argsToParse)
      {
         var parsedArgs = new List<Tuple<IArgument, object>>();

         foreach (var argument in _arguments)
         {
            argument.Parse(argsToParse, parsedArgs);
         }

         return new ParseResponse(parsedArgs.Count == _arguments.Length, parsedArgs.Select(c => c.Item2).ToArray());
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

         consoleWriter.WriteLine("{0} {1}", Name, argNames);
      }

      public void WriteHelp(IWriteToConsole consoleWriter)
      {
         if (!string.IsNullOrEmpty(Description))
         {
            consoleWriter.WriteLine("");
            consoleWriter.WriteLine(Description);
         }

         if (_arguments.Any())
         {
            consoleWriter.WriteLine("");
            consoleWriter.WriteLine("Options:");

            var maxLength = _arguments.Select(arg => arg.ToString().Length).Max();

            foreach (var argument in _arguments)
            {
               var line = argument.ToString();

               line += new string(' ', maxLength - line.Length);

               if (!string.IsNullOrEmpty(argument.Description))
               {
                  line += " " + argument.Description;
               }
               
               consoleWriter.WriteLine("  " + line);
            }
         }

         if (_helpMethod != null)
         {
            consoleWriter.WriteLine("");

            _helpMethod.Invoke(_container, new object[] { consoleWriter });
         }
      }

      public class ParseResponse
      {
         public ParseResponse(bool success, object[] arguments)
         {
            Success = success;
            Arguments = arguments;
         }

         public bool Success { get; }

         public object[] Arguments { get; }
      }
   }
}