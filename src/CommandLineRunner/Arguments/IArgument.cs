namespace CommandLineRunner.Arguments
{
   using System;
   using System.Collections.Generic;

   public interface IArgument
   {
      string Name { get; }

      string Description { get; }

      void Parse(LinkedList<string> argsToParse, List<Tuple<IArgument, object>> parsedArgs);
   }
}