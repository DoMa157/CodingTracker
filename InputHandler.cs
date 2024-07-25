using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class InputHandler
    {

        internal static string GetStringInput(string message)
        {
            string userInput = AnsiConsole.Ask<string>(message);
            return userInput;
        }

        internal static int GetIntInput(string message)
        {
            int userInput = AnsiConsole.Ask<int>(message);
            return userInput;
        }

        internal static string GetDateInput(string message)
        {
            string userInput = AnsiConsole.Ask<string>(message);

            while(!DateTime.TryParseExact(userInput, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _) && userInput.Trim().ToLower() != "q"){
                userInput = AnsiConsole.Ask<string>("[red]Incorrect Format[/]: [yellow]Please enter proper [green]dd-MM-yy HH:mm[/] date:[/]");
            }
            return userInput;
        }
    }
}
