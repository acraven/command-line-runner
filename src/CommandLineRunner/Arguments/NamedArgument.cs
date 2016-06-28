namespace CommandLineParser.Arguments
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.Linq;

   public class NamedArgument : IArgument
   {
      public string Name { get; set; }

      public Type Type { get; set; }

      public bool IsOptional { get; set; }

      public void Parse(LinkedList<string> argsToParse, List<Tuple<IArgument, object>> parsedArgs)
      {
         var skippedArgs = new Stack<string>();
         var found = false;

         while (argsToParse.Any())
         {
            var arg = argsToParse.First.Value;
            argsToParse.RemoveFirst();

            if (string.Compare(arg, $"-{Name}", true, CultureInfo.InvariantCulture) == 0)
            {
               found = true;
               var parsedArg = argsToParse.First.Value.ToParsedArg(Type);
               parsedArgs.Add(new Tuple<IArgument, object>(this, parsedArg));

               argsToParse.RemoveFirst();
               break;
            }

            skippedArgs.Push(arg);
         }

         if (!found && IsOptional)
         {
            parsedArgs.Add(new Tuple<IArgument, object>(this, null));
         }

         while (skippedArgs.Any())
         {
            argsToParse.AddFirst(skippedArgs.Pop());
         }
      }

      public override string ToString()
      {
         if (IsOptional)
         {
            return $"[-{Name} <{Type.Name.ToLower()}>]";
         }

         return $"-{Name} <{Type.Name.ToLower()}>";
      }
   }
}