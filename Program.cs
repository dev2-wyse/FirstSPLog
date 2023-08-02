using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ExceptionFilePath = "C:\\LogFile\\file1.txt";

            static void HandleAndLogException(Exception ex)
            {
                try
                {
                    // Append the exception details to the text file
                    using (StreamWriter writer = File.AppendText(ExceptionFilePath))
                    {
                        writer.WriteLine($"Exception Message: {ex.Message}");
                        writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                        writer.WriteLine();
                        Console.WriteLine("Exception Filled in File");
                    }
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine("Error occurred while writing the exception to the file: " + innerEx.Message);
                }
            }
            try
            {
                string serverName = "192.168.0.3,7998";
                string userName = "sa";
                string password = "@Admin123$$";
                string databaseName = "TestN";

                SqlConnection con = new SqlConnection($"Server={serverName};User Id={userName};Password={password};Database={databaseName};");
                Console.WriteLine(con);
                Console.WriteLine("Start");

                con.Open();

                using (SqlCommand command = new SqlCommand("PersonCity", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@City","Pune");


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                int employeeId = reader.GetInt32(0);
                                string employeeName = reader.GetString(1);
                                string Surname = reader.GetString(2);
                                string Address = reader.GetString(3);
                                string City = reader.GetString(4);
                                Console.WriteLine($"Employee ID: {employeeId}, Employee Surname: {Surname},Employee Address: {Address},Employee Name: {employeeName},Employee City: {City}");

                                string dataLine = $"{employeeId}, {employeeName}, {Surname}, {Address}, {City}";

                                string filePath = "C:\\LogFile\\file.txt";
                                try
                                {

                                    using (StreamWriter writer = File.AppendText(filePath))
                                    {
                                        writer.WriteLine(dataLine);
                                        Console.WriteLine("Data Filled in Txt file");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error occurred while writing to the file: " + ex.Message);
                                    HandleAndLogException(ex);
                                }

                            }
                            Console.WriteLine("End");
                        }
                        else
                        {
                            Console.WriteLine("No data returned from the stored procedure.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database not Connected: " + ex.Message);
                HandleAndLogException(ex);
            }
        }
    }
}


