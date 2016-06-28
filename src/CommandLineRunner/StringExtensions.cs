namespace CommandLineParser
{
   using System;

   public static class StringExtensions
   {
      public static object ToParsedArg(this string arg, Type type)
      {
         if (type == typeof(string))
         {
            return arg;
         }

         if (type == typeof(int))
         {
            return Convert.ToInt32(arg);
         }

         throw new ArgumentOutOfRangeException(nameof(arg), "Unsupported argument type");
      }
   }
}
