namespace CommandLineParser.Tests
{
   public class ContainerWithTwoArgumentVerb
   {
      public int VerbCalledCount { get; private set; }

      public string LastValueA { get; private set; }

      public string LastValueB { get; private set; }

      [Verb]
      public void Verb(string valueA, string valueB)
      {
         VerbCalledCount++;
         LastValueA = valueA;
         LastValueB = valueB;
      }
   }
}
