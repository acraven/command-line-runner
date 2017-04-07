namespace CommandLineParser
{
   using System;

   [AttributeUsage(AttributeTargets.Parameter)]
   public class SensitiveAttribute : Attribute
   {
   }
}