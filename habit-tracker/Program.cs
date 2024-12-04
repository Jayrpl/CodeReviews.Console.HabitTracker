using System;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;

namespace habit_tracker
{
  class Program
  {

    static void Main(string[] args)
    {
      string connectionString = @"Data Source=habit-Tracker.db";

      using (var connection = new SqliteConnection(connectionString))
      {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
          @"CREATE TABLE IF NOT EXISTS drinking_water (
              Id INTEGER PRIMARY KEY AUTOINCREMENT,
              Date TEXT,
              Quantity INTEGER
            )";

        tableCmd.ExecuteNonQuery();

        connection.Close();
      }
    }

    static void GetUserInput()
    {
      Console.Clear();

      bool exit = false;

      while (!exit)
      {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application");
        Console.WriteLine("\nType 1 to View All Records");
        Console.WriteLine("\nType 2 to View Insert Record");
        Console.WriteLine("\nType 3 to View Delete Record");
        Console.WriteLine("\nType 4 to View Update Record");

        int chosenOption;

        while (!int.TryParse(Console.ReadLine(), out chosenOption))
        {
          Console.WriteLine("Please type a number between 0 and 3");
        }

        switch (chosenOption)
        {
          case 0:
            Console.WriteLine("See ya!");
            exit = true;
            break;
          case 1:
            GetAllRecords();
            break;
          case 2:
            Insert();
            break;
          case 3:
            Delete();
            break;
        }
      }
    }
  }
}