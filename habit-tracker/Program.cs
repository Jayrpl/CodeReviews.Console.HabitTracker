using System;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;

namespace habit_tracker
{
  class Program
  {
    static string connectionString = @"Data Source=habit-Tracker.db";

    static void Main(string[] args)
    {

      DoDatabaseCommand(@"CREATE TABLE IF NOT EXISTS drinking_water (
              Id INTEGER PRIMARY KEY AUTOINCREMENT,
              Date TEXT,
              Quantity INTEGER
            )");

      // using (var connection = new SqliteConnection(connectionString))
      // {
      //   connection.Open();
      //   var tableCmd = connection.CreateCommand();

      //   tableCmd.CommandText =
      //     @"CREATE TABLE IF NOT EXISTS drinking_water (
      //         Id INTEGER PRIMARY KEY AUTOINCREMENT,
      //         Date TEXT,
      //         Quantity INTEGER
      //       )";

      //   tableCmd.ExecuteNonQuery();

      //   connection.Close();
      // }
      GetUserInput();
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
            //GetAllRecords();
            break;
          case 2:
            Insert();
            break;
          case 3:
            //Delete();
            break;
            // test
        }
      }
    }

    private static void DoDatabaseCommand(string command)
    {
      using (var connection = new SqliteConnection(connectionString))
      {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = command;
        tableCmd.ExecuteNonQuery();

        connection.Close();
      }
    }

    private static void Insert()
    {
      string date = GetDateInput();

      int quantity = GetNumberInput("\n\nPlease inset number of glasses or other measure of your choice.");

      DoDatabaseCommand($"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})");
    }

    static string GetDateInput()
    {
      Console.WriteLine("Please insert the date in dd/mm/yyyy form.");

      string? input = Console.ReadLine();

      if (input == "0") GetUserInput();

      return input;
    }

    internal static int GetNumberInput(string message)
    {
      Console.WriteLine(message);

      int input;

      while (!int.TryParse(Console.ReadLine(), out input))
      {
        Console.WriteLine(message);
      }

      return input;
    }
  }
}