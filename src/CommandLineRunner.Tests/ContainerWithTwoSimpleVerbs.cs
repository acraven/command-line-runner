namespace CommandLineParser.Tests
{
   public class ContainerWithTwoSimpleVerbs
   {
      public int HashCalledCount { get; private set; }

      public int RandomCalledCount { get; private set; }

      public string LastHashValue { get; private set; }

      public int LastRandomSize { get; private set; }

      [Verb]
      public void Hash(string value)
      {
         HashCalledCount++;
         LastHashValue = value;
      }

      [Verb]
      public void Random(int size)
      {
         RandomCalledCount++;
         LastRandomSize = size;
      }
   }
}