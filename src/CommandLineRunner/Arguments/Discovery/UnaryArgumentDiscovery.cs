namespace CommandLineParser.Arguments.Discovery
{
   using System.Reflection;

   public class UnaryArgumentDiscovery : IDiscoverArguments
   {
      public IArgument Discover(ParameterInfo parameter)
      {
         if (parameter.ParameterType == typeof(bool))
         {
            return new UnaryArgument { Name = parameter.Name.ToCamelCase() };
         }

         return null;
      }
   }
}