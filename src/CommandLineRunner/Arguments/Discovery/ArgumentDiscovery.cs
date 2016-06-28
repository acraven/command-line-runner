namespace CommandLineParser.Arguments.Discovery
{
   using System.Linq;
   using System.Reflection;

   public class ArgumentDiscovery : IDiscoverArguments
   {
      private readonly IDiscoverArguments[] _discoverers;

      public ArgumentDiscovery()
      {
         _discoverers = new IDiscoverArguments[]
            {
               new AnonymousArgumentDiscovery(),
               new UnaryArgumentDiscovery(),
               new NamedArgumentDiscovery()
            };
      }

      public IArgument Discover(ParameterInfo parameter)
      {
         var arguments = _discoverers.Select(d => d.Discover(parameter)).Where(c => c != null).ToArray();
         return arguments.First();
      }
   }
}