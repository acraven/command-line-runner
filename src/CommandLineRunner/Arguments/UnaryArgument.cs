namespace CommandLineRunner.Arguments
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   public class UnaryArgument : IArgument
   {
      public string Name { get; set; }

      public string ShortName { get; set; }

      public void Parse(LinkedList<string> argsToParse, List<Tuple<IArgument, object>> parsedArgs)
      {
         var skippedArgs = new Stack<string>();
         var found = false;

         while (argsToParse.Any())
         {
            var arg = argsToParse.First.Value;
            argsToParse.RemoveFirst();

            if (string.Compare(arg, $"--{Name}", StringComparison.OrdinalIgnoreCase) == 0 || (!string.IsNullOrEmpty(ShortName) && string.Compare(arg, $"-{ShortName}", StringComparison.OrdinalIgnoreCase) == 0))
            {
               found = true;
               break;
            }

            skippedArgs.Push(arg);
         }

         parsedArgs.Add(new Tuple<IArgument, object>(this, found));

         while (skippedArgs.Any())
         {
            argsToParse.AddFirst(skippedArgs.Pop());
         }
      }

      public override string ToString()
      {
         var arg = $"--{Name}";

         if (!string.IsNullOrEmpty(ShortName))
         {
            arg += $"|-{ShortName}";
         }

         return $"[{arg}]";
      }
   }
}