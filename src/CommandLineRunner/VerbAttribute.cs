namespace CommandLineRunner
{
   using System;

   [AttributeUsage(AttributeTargets.Method)]
   public class VerbAttribute : Attribute
   {
      public string Description { get; set; }
   }
}
