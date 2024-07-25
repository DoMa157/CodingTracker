using System.Configuration;
using Dapper;
using System.Data.SQLite;
using Spectre.Console;

namespace CodingTracker
{
    internal class Controller
    {
        static string? path = ConfigurationManager.AppSettings.Get("path");
        static string? dbname = ConfigurationManager.AppSettings.Get("dbname");
        static string connectionString = $@"Data source={path}{dbname};";
        static void Main(string[] args)
        {
            InitializeTable();
            MainLoop();
        }

        internal static void MainLoop()
        {
            var closeConsole = false;
            while (!closeConsole)
            {
                AnsiConsole.Write(new FigletText("Coding Tracker").Color(Color.White).LeftJustified());
                var promptChoices = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[yellow]Choose Command:[/]").PageSize(10)
                    .AddChoices(new[]
                    {
                        "INSERT RECORD", "UPDATE RECORD", "VIEW RECORDS", "DELETE RECORD", "QUIT"
                    }).HighlightStyle(new Style(Color.Black, Color.White)));
                switch (promptChoices)
                {
                    case "INSERT RECORD":
                        InsertRecord();
                        break;
                    case "UPDATE RECORD":
                        UpdateRecord();
                        break;
                    case "VIEW RECORDS":
                        ViewRecords();
                        break;
                    case "DELETE RECORD":
                        DeleteRecord();
                        break;
                    case "QUIT":
                        closeConsole = true;
                        AnsiConsole.Write("Have a great day!");
                        Environment.Exit(0);
                        break;
                }
            }
        }
        internal static void InitializeTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_stats(Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT, EndTime TEXT)";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal static void UpdateRecord()
        {
            AnsiConsole.Clear();
            ViewRecords();
            using (var connection = new SQLiteConnection(connectionString))
            {
                int userId = InputHandler.GetIntInput("[yellow]Enter id of entry to be updated:[/]");
                var sql = $"SELECT * FROM coding_stats where id = {userId}";
                int queryAmount = connection.ExecuteScalar<int>(sql);
                while (queryAmount == 0)
                {
                    userId = InputHandler.GetIntInput($"[red]{userId}[/] doesn't exist in database. Please try a different ID\n");
                    sql = $"SELECT * FROM coding_stats where id = {userId}";
                    queryAmount = connection.ExecuteScalar<int>(sql);
                }
                string startDate = InputHandler.GetDateInput("[yellow]Enter new start date:[/]");
                string endDate = InputHandler.GetDateInput("[yellow]Enter new end date:[/]");
                Validator.ValidateEndDate(startDate, endDate);
                if (startDate.Trim().ToLower() == "q" || endDate.Trim().ToLower() == "q")
                {
                    MainLoop();
                    return;
                }
                sql = @$"UPDATE coding_stats SET StartTime = '{startDate}', EndTime = '{endDate}'";
                connection.Execute(sql);
            }
            MainLoop();

        }
        internal static void ViewRecords()
        {
            AnsiConsole.Clear();
            using (var connection = new SQLiteConnection(connectionString))
            {
                var sql = $@"SELECT * FROM coding_stats";
                var codingSessions = connection.Query<CodingSession>(sql).ToList();
                var table = new Table();
                table.AddColumn("ID");
                table.AddColumn("Start Time");
                table.AddColumn("End Time");
                table.AddColumn("Duration");
                foreach (var codingSession in codingSessions)
                {
                    table.AddRow($"[green]{codingSession.Id}[/]", $"{codingSession.StartTime.ToString("dd-MM-yy HH:mm")}", $"{codingSession.EndTime.ToString("dd-MM-yy HH:mm")}", $"{codingSession.Duration}");
                }
                AnsiConsole.Write(table);
            }
        }
        internal static void DeleteRecord()
        {
            AnsiConsole.Clear();
            ViewRecords();
            using (var connection = new SQLiteConnection(connectionString))
            {
                int userId = InputHandler.GetIntInput("[yellow]Enter id of entry to be updated:[/]");
                var sql = $"SELECT * FROM coding_stats where id = {userId}";
                int queryAmount = connection.ExecuteScalar<int>(sql);
                while (queryAmount == 0)
                {
                    userId = InputHandler.GetIntInput($"[red]{userId}[/] doesn't exist in database. Please try a different ID\n");
                    sql = $"SELECT * FROM coding_stats where id = {userId}";
                    queryAmount = connection.ExecuteScalar<int>(sql);
                }
                sql = @$"DELETE FROM coding_stats WHERE Id = '{userId}'";
                connection.Execute(sql);
            }
            MainLoop();
        }
        internal static void InsertRecord()
        {
            AnsiConsole.Clear();
            ViewRecords();
            using (var connection = new SQLiteConnection(connectionString))
            {
                string startDate = InputHandler.GetDateInput("[yellow]Enter start date of entry ([green]dd-MM-yy HH:mm[/]):[/]");
                string endDate = InputHandler.GetDateInput("[yellow]Enter end date of entry ([green]dd-MM-yy HH:mm[/]):[/]");
                endDate = Validator.ValidateEndDate(startDate, endDate);
                if (startDate.Trim().ToLower() == "q" || endDate.Trim().ToLower() == "q")
                {
                    MainLoop();
                    return;
                }
                var sql = $@"INSERT INTO coding_stats(StartTime, EndTime) VALUES ('{startDate}', '{endDate}')";
                connection.Execute(sql);
            }
            MainLoop();
        }
    }
}
