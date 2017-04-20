namespace CommandLineRunner.Arguments.Discovery
{
   using System.Linq;
   using System.Reflection;

   public class AnonymousArgumentDiscovery : IDiscoverArguments
   {
      public IArgument Discover(ParameterInfo parameter)
      {
         var anonymousAttribute = parameter.GetCustomAttributes(typeof(AnonymousAttribute)).SingleOrDefault();

         if (anonymousAttribute == null)
         {
            return null;
         }

         var argumentAttribute = parameter.GetCustomAttributes(typeof(ArgumentAttribute)).Cast<ArgumentAttribute>().SingleOrDefault();

         return new AnonymousArgument
         {
            Name = parameter.Name.ToCamelCase(),
            Description = argumentAttribute?.Description,
            Type = parameter.ParameterType
         };
      }
   }
}