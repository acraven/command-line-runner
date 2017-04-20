namespace CommandLineRunner
{
   public interface IWriteToConsole
   {
      void Write(string format, params object[] args);

      void WriteLine(string format, params object[] args);
   }
}
