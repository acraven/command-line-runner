namespace CommandLineParser.Arguments.Discovery
{
   using System;
   using System.Reflection;

   public class NamedArgumentDiscovery : IDiscoverArguments
   {
      public IArgument Discover(ParameterInfo parameter)
      {
         var argumentType = GetArgumentType(parameter.ParameterType);

         return new NamedArgument
         {
            Name = parameter.Name.ToCamelCase(),
            Type = argumentType,
            IsOptional = parameter.HasDefaultValue
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