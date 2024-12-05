using System;
using System.Globalization;
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
            Environment.Exit(0);
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
          case 4:
            Update();
            break;
          default:
            Console.WriteLine("\nInvalid command. Please type a number from 0 to 4.\n");
            break;
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

    private static void Delete()
    {
      Console.Clear();
      GetAllRecords();

      var recordId = GetNumberInput("\n\nPlease input the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

      using (var connection = new SqliteConnection(connectionString))
      {
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
          Console.WriteLine($"Record with Id {recordId} does not exist. \n\n");
          Delete();
        }
      }

      Console.WriteLine($"\n\nRecord Id with {recordId} was successfully deleted.");

      GetUserInput();
    }

    private static void Update()
    {
      GetAllRecords();

      var recordId = GetNumberInput("\n\nPlease input the Id of the record you want to update or type 0 to go back to Main Menu\n\n");

      using (var connection = new SqliteConnection(connectionString))
      {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
          Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
          connection.Close();
          Update();
        }

        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

        tableCmd.ExecuteNonQuery();

        connection.Close();
      }
    }

    private static void GetAllRecords()
    {
      Console.Clear();

      using (var connection = new SqliteConnection(connectionString))
      {
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM drinking_water ";

        List<DrinkingWater> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
          while (reader.Read())
          {
            tableData.Add(
            new DrinkingWater
            {
              Id = reader.GetInt32(0),
              Date = DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy", CultureInfo.CurrentCulture),
              Quantity = reader.GetInt32(2)
            });
          }
        }
        else
        {
          Console.WriteLine("No rows found.");
        }

        connection.Close();

        Console.WriteLine($"-------------------------------------------------------\n");

        foreach (var data in tableData)
        {
          Console.WriteLine($"{data.Id} - {data.Date.ToString("dd/MM/yyyy")} - Quantity: {data.Quantity}");
        }
        Console.WriteLine($"-------------------------------------------------------\n");


      }


    }

    static string GetDateInput()
    {
      Console.WriteLine("Please insert the date in dd/mm/yyyy form.");

      string? input = Console.ReadLine();

      if (input == "0") GetUserInput();

      while (!DateTime.TryParseExact(input, "dd/mm/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
      {
        Console.WriteLine("\n\nInvalid Date. Please insert the date in dd/mm/yyyy form.\n\n");
        input = Console.ReadLine();
      }

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

  public class DrinkingWater
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public int Quantity { get; set; }
  }

}