namespace CommandLineRunner.Arguments
{
   using System;
   using System.Collections.Generic;

   public class AnonymousArgument : IArgument
   {
      public Type Type { get; set; }

      public void Parse(LinkedList<string> argsToParse, List<Tuple<IArgument, object>> parsedArgs)
      {
         var parsedArg = argsToParse.First.Value.ToParsedArg(Type);
         parsedArgs.Add(new Tuple<IArgument, object>(this, parsedArg));
         argsToParse.RemoveFirst();
      }

      public override string ToString()
      {
         return $"<{Type.Name.ToLower()}>";
      }
   }
}