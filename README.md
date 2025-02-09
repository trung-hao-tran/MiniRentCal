# Rental Management System

A rental management system to handle properties, utilities, and expenses. The system is deployed via Azure Web Service for scalability and reliability.

- **Modular Back-End:** Built with ASP.NET MVC and Entity Framework, integrated with Azure SQL for secure and efficient data storage.
- **Dynamic Cost Calculation:** Implements real-time UI updates using JavaScript to improve operational efficiency.
- **Custom Invoicing:** Features custom invoicing templates that streamline rental cost calculations and invoicing processes.

## Prerequisites

Before running the application using Docker, please ensure that you have the following installed:

- **Docker:**  
  [Install Docker Desktop](https://docs.docker.com/get-docker/)  
  (Docker Compose is included with Docker Desktop.)
- **.NET SDK:**  
  [Download .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **SQL Server Container:**  
  The application uses the SQL Server container image ([mcr.microsoft.com/mssql/server:2019-latest](https://hub.docker.com/_/microsoft-mssql-server)); no additional SQL Server installation is required.

## Running the Application with Docker Compose

The project uses a Docker Compose file to run both the ASP.NET web application and SQL Server. Follow these steps:

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/trung-hao-tran/MiniRentCal.git
   cd MiniRentalCal
   ```

2. **Prepare the HTTPS Certificate:**

	``` powershell
	dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p YourPasswordHere
	dotnet dev-certs https --trust
	```
	if `command prompt`:
	``` bash
	dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p YourPasswordHere
	dotnet dev-certs https --trust
	```
	
	You can change the password to your preference. The password should be matched in the `docker-compose.yml` file.

3. **Build and Run the Containers:**

   ```bash
   docker-compose up --build
   ```

4. **Bacpac Import/Export**

Bacpac file is provided in Data. To import the bacpac file, please use [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

Please keep the database name as `Azure_backup` for the application to work properly.

If you wish to use different database, please update the connection string in `appsettings.json` file.


5. **Access the Application:**
   - **HTTPS:** Open [https://localhost:8080](https://localhost:8080)

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [.NET Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQLPackage Documentation](https://learn.microsoft.com/en-us/sql/tools/sqlpackage)

## License

This project is licensed under the [MIT License](LICENSE).

