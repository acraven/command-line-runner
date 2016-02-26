using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineParser
{
   public class Runner
   {
      private readonly IDictionary<string, Verb> _verbs = new Dictionary<string, Verb>();

      public IWriteToConsole ConsoleWriter { get; set; } = new ConsoleWriter();

      public string Name { get; set; } = AppDomain.CurrentDomain.FriendlyName;

      public int Run(string[] args)
      {
         Verb verb;

         if (args.Length == 0 || !_verbs.TryGetValue(args[0], out verb))
         {
            WriteUsage();
            return 1;
         }

         var i = 1;
         var values = new Dictionary<string, string>();

         while (i < args.Length)
         {
            if (args[i].StartsWith("-"))
            {
               values.Add(args[i].Substring(1), args[i + 1]);
               i += 2;
            }
            else
            {
               values.Add(string.Empty, args[i]);
               i++;
            }
         }

         var typedArgs = verb.Arguments.Select(a => ExtractArg(a, values)).ToArray();
         verb.MethodInfo.Invoke(verb.Container, typedArgs);

         return 0;
      }

      private object ExtractArg(Argument arg, IDictionary<string, string> values)
      {
         string value;

         if (!values.TryGetValue(arg.IsKeyed ? arg.Name : string.Empty, out value))
         {
            return null;
         }

         if (arg.Type == typeof(string))
         {
            return value;
         }

         if (arg.Type == typeof(int))
         {
            return Convert.ToInt32(value);
         }

         throw new ArgumentOutOfRangeException(nameof(arg), "Unsupported argument type");
      }

      private void WriteUsage()
      {
         ConsoleWriter.Write("USAGE: {0}", Name);

         foreach (var verb in _verbs.Values)
         {
            var argNames = string.Join(" ", verb.Arguments.Select(FormatArg));

            ConsoleWriter.Write("  {0} {1}", verb.Name, argNames);
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
               var verb = new Verb { Name = ConvertCase(methodInfo.Name) };
               _verbs.Add(verb.Name, verb);

               var parameters = methodInfo.GetParameters();
               var isKeyed = parameters.Length > 1;
               verb.Arguments = parameters.Select(p => CreateArgument(p, isKeyed)).ToArray();
               verb.MethodInfo = methodInfo;
               verb.Container = container;
            }
         }
      }

      private static Argument CreateArgument(ParameterInfo parameterInfo, bool isKeyed)
      {
         return new Argument
         {
            Name = ConvertCase(parameterInfo.Name),
            Type = GetArgumentType(parameterInfo.ParameterType),
            IsKeyed = isKeyed,
            IsOptional = parameterInfo.HasDefaultValue
         };
      }

      private static Type GetArgumentType(Type type)
      {
         if (type == typeof(string))
         {
            return type;
         }

         var nullableType = Nullable.GetUnderlyingType(type);

         if (nullableType != null)
         {
            return nullableType;
         }

         return type;
      }

      private static string ConvertCase(string source)
      {
         return char.ToLower(source[0]) + source.Substring(1);
      }

      private static string FormatArg(Argument arg)
      {
         var result = arg.Type.Name.ToLower();

         if (arg.IsKeyed)
         {
            result = $"-{arg.Name} <{result}>";
         }
         else
         {
            result = $"<{arg.Name}:{result}>";
         }

         if (arg.IsOptional)
         {
            result = $"[{result}]";
         }

         return result;
      }

      private class Verb
      {
         public string Name { get; set; }

         public Argument[] Arguments { get; set; }

         public MethodInfo MethodInfo { get; set; }

         public object Container { get; set; }
      }

      private class Argument
      {
         public string Name { get; set; }

         public Type Type { get; set; }

         public bool IsKeyed { get; set; }

         public bool IsOptional { get; set; }
      }
   }
}
