namespace CommandLineParser
{
   public interface IWriteToConsole
   {
      void Write(string message);

      void Write(string format, params object[] args);
   }
}
