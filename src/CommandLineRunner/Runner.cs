namespace CommandLineParser
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;

   public class Runner
   {
      private readonly IDictionary<string, Verb> _verbs = new Dictionary<string, Verb>();

      public IWriteToConsole ConsoleWriter { get; set; } = new ConsoleWriter();

      public string Name { get; set; } = AppDomain.CurrentDomain.FriendlyName;

      public int Run(string[] args)
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

            verb.Run(verbArgs);
            return 0;
         }
         catch (Exception e)
         {
            ConsoleWriter.Write(e.Message);
            return 1;
         }
      }

      private void WriteUsage()
      {
         ConsoleWriter.Write("USAGE: {0}", Name);

         foreach (var verb in _verbs.Values)
         {
            verb.WriteUsage(ConsoleWriter);
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
               var verb = new Verb(methodInfo, container);

               _verbs.Add(verb.Name, verb);
            }
         }
      }
   }
}
