namespace CommandLineRunner.Arguments.Discovery
{
   using System.Linq;
   using System.Reflection;

   public class AnonymousArgumentDiscovery : IDiscoverArguments
   {
      public IArgument Discover(ParameterInfo parameter)
      {
         var anonymousAttribute = parameter.GetCustomAttributes(typeof(AnonymousAttribute)).SingleOrDefault();

         if (anonymousAttribute != null)
         {
            return new AnonymousArgument { Type = parameter.ParameterType };
         }

         return null;
      }
   }
}