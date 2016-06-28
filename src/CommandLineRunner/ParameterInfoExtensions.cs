namespace CommandLineParser
{
   public static class ParameterInfoExtensions
   {
      public static string ToCamelCase(this string source)
      {
         return char.ToLower(source[0]) + source.Substring(1);
      }
   }
}