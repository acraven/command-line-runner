namespace CommandLineRunner
{
   using System;

   [AttributeUsage(AttributeTargets.Parameter)]
   public class SensitiveAttribute : Attribute
   {
   }
}