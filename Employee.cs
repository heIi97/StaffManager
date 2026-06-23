using System;

namespace StaffManager
{
    // Represents a single employee
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string Country { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public string FullInfo => $"{FirstName} {LastName} - {Position ?? "No position"} ({Country}), Salary: {Salary} PLN";
    }
}