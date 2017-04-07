namespace CommandLineRunner.Arguments.Discovery
{
   using System.Reflection;

   public interface IDiscoverArguments
   {
      IArgument Discover(ParameterInfo parameter);
   }
}