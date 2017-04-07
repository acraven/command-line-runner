namespace CommandLineRunner
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using System.Threading.Tasks;
   using CommandLineRunner.Arguments.Discovery;

   public class Runner
   {
      private readonly IDictionary<string, Verb> _verbs = new Dictionary<string, Verb>();
      private readonly string _name;
      private readonly IWriteToConsole _consoleWriter;
      private readonly IDiscoverArguments _argumentDiscovery;

      public Runner(string name, IWriteToConsole consoleWriter, IDiscoverArguments argumentDiscovery)
      {
         _name = name;
         _consoleWriter = consoleWriter;
         _argumentDiscovery = argumentDiscovery;
      }

      public async Task<int> RunAsync(string[] args)
      {
         try
         {
            Verb verb;

            if (args.Length == 0 || !_verbs.TryGetValue(args[0], out verb))
            {
               WriteUsage();
               return 1;
            }

            var argsToParse = new LinkedList<string>(args.Skip(1));
            var verbArgs = verb.ParseArguments(argsToParse);

            if (argsToParse.Any())
            {
               WriteUsage();
               return 1;
            }

            await verb.RunAsync(verbArgs);
            return 0;
         }
         catch (Exception e)
         {
            _consoleWriter.Write(e.Message);
            return 1;
         }
      }

      private void WriteUsage()
      {
         _consoleWriter.Write("USAGE: {0}", _name);

         foreach (var verb in _verbs.Values)
         {
            verb.WriteUsage(_consoleWriter);
         }
      }

      public void Register<TContainer>(TContainer container)
      {
         if (container == null) throw new ArgumentNullException(nameof(container));

         var methods = container.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

         foreach (var methodInfo in methods)
         {
            var attributes = methodInfo.GetCustomAttributes(typeof(VerbAttribute)).ToArray();

            if (attributes.Length != 0)
            {
               var verb = new Verb(methodInfo, container, _argumentDiscovery);

               _verbs.Add(verb.Name, verb);
            }
         }
      }
   }
}
