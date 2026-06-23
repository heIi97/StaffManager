using System;
using Microsoft.Extensions.Configuration;

namespace StaffManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Config
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            string connectionString = config["ConnectionString"] ?? throw new Exception("Missing ConnectionString in configuration!");
            string filePath = config["CsvFilePath"] ?? throw new Exception("Missing CSV file path in configuration!");

            var fileImporter = new FileImporter();
            var dbRepository = new DatabaseRepository(connectionString);

            Console.WriteLine("""
                 Available commands:
                 import - Clear the database and import new data from the CSV file
                 merge  - Add new data from the CSV file to the database without deleting existing records
                 show   - Display all employees from the database
                 exit   - Close the app
                 """);
           
            bool isRunning = true;
            while (isRunning)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                try
                {
                    switch (input)
                    {
                        // Import data from .CSV (keeps old records)
                        case "merge":
                            {
                                Console.WriteLine("Importing data from CSV file.");
                                var importedEmployees = fileImporter.ImportFromCsv(filePath);

                                if (importedEmployees.Count > 0)
                                {
                                    dbRepository.InsertEmployees(importedEmployees);
                                    Console.WriteLine($"{importedEmployees.Count} records imported to the database.");
                                }
                                else
                                {
                                    Console.WriteLine("Import failed or file is empty.");
                                }
                                break;
                            }

                        // Import data from .CSV (deletes old records)
                        case "import":
                            {
                                Console.WriteLine("Importing data from CSV file.");
                                var importedEmployees = fileImporter.ImportFromCsv(filePath);

                                if (importedEmployees.Count > 0)
                                {
                                    dbRepository.ClearTable();
                                    dbRepository.InsertEmployees(importedEmployees);
                                    Console.WriteLine($"{importedEmployees.Count} records imported to the database.");
                                }
                                else
                                {
                                    Console.WriteLine("Import failed or file is empty.");
                                }
                                break;
                            }

                        // Get full data from the database
                        case "show":
                            Console.WriteLine("Getting records from the database...");
                            var dbEmployees = dbRepository.GetAllEmployees();

                            if (dbEmployees.Count == 0)
                            {
                                Console.WriteLine("Database is empty.");
                            }
                            else
                            {
                                foreach (var emp in dbEmployees)
                                {
                                    Console.WriteLine($"    {emp.FullInfo}");
                                }
                                Console.WriteLine($"\nTotal records: {dbEmployees.Count}");
                            }
                            break;

                        case "exit" or "quit":
                            isRunning = false;
                            break;

                        default:
                            if (!string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine($"""
                                     Unknown command: '{input}'.
                                     Available commands:
                                     import - Clear the database and import new data from the CSV file
                                     merge  - Add new data from the CSV file to the database without deleting existing records
                                     show   - Display all employees from the database
                                     exit   - Close the app
                                     """);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] An error occurred: {ex.Message}");
                }
            }
        }
    }
}