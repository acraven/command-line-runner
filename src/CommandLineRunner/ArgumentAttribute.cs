namespace CommandLineRunner
{
   using System;

   [AttributeUsage(AttributeTargets.Parameter)]
   public class ArgumentAttribute : Attribute
   {
      public string ShortName { get; set; }

      public string Description { get; set; }
   }
}