namespace CommandLineRunner
{
   using System;

   [AttributeUsage(AttributeTargets.Parameter)]
   public class ArgumentAttribute : Attribute
   {
      public string ShortName { get; set; }
   }
}