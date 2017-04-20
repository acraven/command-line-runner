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
            if (args.Length == 0)
            {
               WriteUsage();
               return 1;
            }

            var help = string.Compare(args[0], "--help", StringComparison.OrdinalIgnoreCase) == 0 ||
                       string.Compare(args[0], "-h", StringComparison.OrdinalIgnoreCase) == 0;

            if (help && args.Length == 1)
            {
               WriteUsage();
               return 1;
            }

            var verbIndex = help ? 1 : 0;
            Verb verb;

            if (!_verbs.TryGetValue(args[verbIndex], out verb))
            {
               WriteUsage();
               return 1;
            }

            if (help)
            {
               WriteVerbHelp(verb);
               return 1;
            }

            var argsToParse = new LinkedList<string>(args.Skip(verbIndex + 1));
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
            _consoleWriter.WriteLine(e.Message);
            return 1;
         }
      }

      private void WriteUsage()
      {
         _consoleWriter.WriteLine("USAGE: {0} [--help|-h]", _name);
         _consoleWriter.WriteLine("{0}<command> [<args>]", new string(' ', _name.Length + 8));

         if (!_verbs.Any())
         {
            return;
         }

         _consoleWriter.WriteLine("");
         _consoleWriter.WriteLine("These are the available commands:");

         foreach (var verb in _verbs.Values)
         {
            _consoleWriter.Write("  ");
            verb.WriteUsage(_consoleWriter);
         }
      }

      private void WriteVerbHelp(Verb verb)
      {
         _consoleWriter.Write("USAGE: {0} ", _name);
         verb.WriteUsage(_consoleWriter);
         verb.WriteHelp(_consoleWriter);
      }

      public void Register<TContainer>(TContainer container)
      {
         if (container == null) throw new ArgumentNullException(nameof(container));

         var methods = container.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

         foreach (var methodInfo in methods)
         {
            var verbAttribute = methodInfo.GetCustomAttributes(typeof(VerbAttribute)).Cast<VerbAttribute>().FirstOrDefault();

            if (verbAttribute != null)
            {
               var verb = new Verb(verbAttribute.Description, methodInfo, container, _argumentDiscovery);

               _verbs.Add(verb.Name, verb);
            }
         }
      }
   }
}
