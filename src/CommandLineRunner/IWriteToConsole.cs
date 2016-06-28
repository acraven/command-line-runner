namespace CommandLineParser
{
   public interface IWriteToConsole
   {
      void Write(string format, params object[] args);
   }
}
