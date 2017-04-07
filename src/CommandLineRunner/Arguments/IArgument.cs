namespace CommandLineRunner.Arguments
{
   using System;
   using System.Collections.Generic;

   public interface IArgument
   {
      void Parse(LinkedList<string> argsToParse, List<Tuple<IArgument, object>> parsedArgs);
   }
}