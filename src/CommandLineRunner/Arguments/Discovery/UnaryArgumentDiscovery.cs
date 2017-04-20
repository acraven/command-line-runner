namespace CommandLineRunner.Arguments.Discovery
{
   using System.Linq;
   using System.Reflection;

   public class UnaryArgumentDiscovery : IDiscoverArguments
   {
      public IArgument Discover(ParameterInfo parameter)
      {
         if (parameter.ParameterType == typeof(bool))
         {
            var argumentAttribute = parameter.GetCustomAttributes(typeof(ArgumentAttribute)).Cast<ArgumentAttribute>().SingleOrDefault();

            return new UnaryArgument
            {
               Name = parameter.Name.ToCamelCase(),
               Description = argumentAttribute?.Description,
               ShortName = argumentAttribute?.ShortName?.ToLower()
            };
         }

         return null;
      }
   }
}