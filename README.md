# StaffManager

Console application designed for importing, merging, and managing employee records from CSV files into a local SQL Server database.

- Framework: .NET 10
- Language: C#
- Database: Microsoft SQL Server (LocalDB)
- Libraries (NuGet):
  - Microsoft.Data.SqlClient
  - Microsoft.Extensions.Configuration
  - Microsoft.Extensions.Configuration.Json

## Setup

### 1. Database setup
The application connects to Visual Studio's built-in LocalDB.
Open SQL Server Object Explorer > `(localdb)\MSSQLLocalDB` > Create a database named `CompanyDB` and execute the script:

```sql
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Position NVARCHAR(50) NULL,
    Country NVARCHAR(50) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    HireDate DATE NOT NULL
);
```

### 2. Config
Ensure your appsettings.json file is located in the root directory of your project. ConnectionString and CsvFilePath parameters must be set:
```json
{
  "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CompanyDB;Integrated Security=True",
  "CsvFilePath": "YOUR CSV FILE PATH e.g. C:\\data\\employees.csv"
}
```

### 3. CSV File format
The application expects a CSV file using a semicolon (;) separator, Position field is optional and supports empty values.

## Commands:
- import - Wipes the target table and imports all records from the configured CSV file.
- merge - Appends all CSV records into the database while preserving existing data.
- show - Lists all stored employees along with the total count.
- exit / quit - Closes the application.