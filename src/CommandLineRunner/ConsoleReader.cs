namespace CommandLineParser
{
   using System;

   public class ConsoleReader : IReadFromConsole
   {
      // http://stackoverflow.com/questions/29201697/hide-replace-when-typing-a-password-c
      public string ReadSensitive(string prompt)
      {
         Console.Write($"{prompt}:");
         var info = Console.ReadKey(true);

         var password = "";

         while (info.Key != ConsoleKey.Enter)
         {
            if (info.Key != ConsoleKey.Backspace)
            {
               Console.Write("*");
               password += info.KeyChar;
            }
            else if (info.Key == ConsoleKey.Backspace)
            {
               if (!string.IsNullOrEmpty(password))
               {
                  // remove one character from the list of password characters
                  password = password.Substring(0, password.Length - 1);
                  // get the location of the cursor
                  int pos = Console.CursorLeft;
                  // move the cursor to the left by one character
                  Console.SetCursorPosition(pos - 1, Console.CursorTop);
                  // replace it with space
                  Console.Write(" ");
                  // move the cursor to the left by one character again
                  Console.SetCursorPosition(pos - 1, Console.CursorTop);
               }
            }
            info = Console.ReadKey(true);
         }

         // add a new line because user pressed enter at the end of their password
         Console.WriteLine();

         return password;
      }
   }
}