namespace CommandLineRunner.Arguments.Discovery
{
   using System;
   using System.Linq;
   using System.Reflection;

   public class NamedArgumentDiscovery : IDiscoverArguments
   {
      private readonly IReadFromConsole _consoleReader;

      public NamedArgumentDiscovery(IReadFromConsole consoleReader)
      {
         _consoleReader = consoleReader;
      }

      public IArgument Discover(ParameterInfo parameter)
      {
         var argumentType = GetArgumentType(parameter.ParameterType);
         var sensitiveAttribute = parameter.GetCustomAttributes(typeof(SensitiveAttribute)).SingleOrDefault();
         var argumentAttribute = parameter.GetCustomAttributes(typeof(ArgumentAttribute)).Cast<ArgumentAttribute>().SingleOrDefault();

         return new NamedArgument(_consoleReader)
         {
            Name = parameter.Name.ToCamelCase(),
            Description = argumentAttribute?.Description,
            ShortName = argumentAttribute?.ShortName?.ToLower(),
            Type = argumentType,
            IsOptional = parameter.HasDefaultValue,
            IsSensitive = sensitiveAttribute != null
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
   }
}