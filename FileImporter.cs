using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StaffManager
{
    //Imports all employees from a specified CSV file
    public class FileImporter
    {
        public List<Employee> ImportFromCsv(string filePath)
        {
            var employees = new List<Employee>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return employees;
            }

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            //i=0 CSV headers
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    // Mapping values
                    string[] cells = lines[i].Split(';');
                    var employee = new Employee
                    {
                        FirstName = cells[0],
                        LastName = cells[1],
                        Position = string.IsNullOrWhiteSpace(cells[2]) ? null : cells[2],
                        Country = cells[3],
                        Salary = Convert.ToDecimal(cells[4]),
                        HireDate = Convert.ToDateTime(cells[5])
                    };

                    employees.Add(employee);
                }
                catch (Exception ex)
                {
                    // Fail > log and continue with next one
                    File.AppendAllText("error_log.txt", $"Error line {i}: {ex.Message}{Environment.NewLine}");
                }
            }

            return employees;
        }
    }
}