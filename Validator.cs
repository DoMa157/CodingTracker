using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Validator
    {
        private static bool ValidateDates(string startDate, string endDate)
        {
            return (DateTime.ParseExact(endDate, "dd-MM-yy HH:mm", null).Subtract(DateTime.ParseExact(startDate, "dd-MM-yy HH:mm", null)).TotalDays > 0);
        }


        internal static string ValidateEndDate(string startDate, string endDate)
        {
            while (!ValidateDates(startDate, endDate) && endDate.Trim().ToLower() != "q")
            {
                endDate = InputHandler.GetDateInput($"[red]{endDate}[/] is earlier than [red]{startDate}[/], Please Provide a correct [green]dd-MM-yy[/] HH:mm date.\n");
            }
            return endDate;
        }
    }
}
