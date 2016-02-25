namespace CommandLineParser.Tests
{
   public class ContainerWithOptionalTwoArgumentVerb
   {
      public int VerbCalledCount { get; private set; }

      public string LastStringValue { get; private set; }

      public int? LastIntValue { get; private set; }

      [Verb]
      public void Verb(string stringValue = null, int? intValue = null)
      {
         VerbCalledCount++;
         LastStringValue = stringValue;
         LastIntValue = intValue;
      }
   }
}
