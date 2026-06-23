using System;
using Microsoft.Data.SqlClient;

namespace StaffManager
{
    public class DatabaseRepository
    {
        private readonly string _connectionString;

        public DatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Removes all records from employees table
        public void ClearTable()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("TRUNCATE TABLE Employees", connection);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // Inserts a list of employees
        public void InsertEmployees(List<Employee> employees)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            foreach (var emp in employees)
            {
                string sql = @"INSERT INTO Employees (FirstName, LastName, Position, Country, Salary, HireDate) 
                                           VALUES (@FirstName, @LastName, @Position, @Country, @Salary, @HireDate)";

                using var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@FirstName", emp.FirstName);
                command.Parameters.AddWithValue("@LastName", emp.LastName);
                command.Parameters.AddWithValue("@Position", emp.Position ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Country", emp.Country);
                command.Parameters.AddWithValue("@Salary", emp.Salary);
                command.Parameters.AddWithValue("@HireDate", emp.HireDate);

                command.ExecuteNonQuery();
            }
        }

        // Returns all employees records
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT Id, FirstName, LastName, Position, Country, Salary, HireDate FROM Employees", connection);

            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Position = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Country = reader.GetString(4),
                    Salary = reader.GetDecimal(5),
                    HireDate = reader.GetDateTime(6)
                });
            }

            return employees;
        }
    }
}