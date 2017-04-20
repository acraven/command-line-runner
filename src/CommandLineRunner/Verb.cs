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

            foreach (var argument in _arguments)
            {
               consoleWriter.WriteLine("  {0}", argument.ToString());
            }
         }

         if (_helpMethod != null)
         {
            consoleWriter.WriteLine("");

            _helpMethod.Invoke(_container, new object[] { consoleWriter });
         }
      }
   }
}