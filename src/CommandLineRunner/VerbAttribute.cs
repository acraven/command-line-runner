using System;

namespace CommandLineParser
{
   [AttributeUsage(AttributeTargets.Method)]
   public class VerbAttribute : Attribute
   {
   }

   [AttributeUsage(AttributeTargets.Parameter)]
   public class AnonymousAttribute : Attribute
   {
   }
}
