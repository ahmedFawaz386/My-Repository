using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoBy_Driver_ID(int _Driver_ID, 
            ref int _Person_ID,ref int _CreatedByUser_ID,ref DateTime _CreatedDate )
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Drivers WHERE DriverID = @_Driver_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                    _Person_ID = (int)reader["PersonID"];
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];
                    _CreatedDate = (DateTime)reader["CreatedDate"];


                }
                    else
                    {
                        // The record was not found
                        isFound = false;
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    isFound = false;
                }
                finally
                {
                    connection.Close();
                }

                return isFound;
            }

        public static bool GetDriverInfoBy_Person_ID(int _Person_ID,ref int _Driver_ID,
            ref int _CreatedByUser_ID,ref DateTime _CreatedDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Drivers WHERE PersonID = @_Person_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    _Driver_ID = (int)reader["DriverID"];
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];
                    _CreatedDate = (DateTime)reader["CreatedDate"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllDrivers()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Drivers_View order by FullName";
           
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)

                    {
                        dt.Load(reader);
                    }

                    reader.Close();


                }

                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return dt;

            }

        public static int AddNewDriver( int _Person_ID, int _CreatedByUser_ID)
        {
            int _Driver_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@_Person_ID,@_CreatedByUser_ID,@_CreatedDate);
                          
                            SELECT SCOPE_IDENTITY();";
         

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
            command.Parameters.AddWithValue("@_CreatedDate", DateTime.Now);
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _Driver_ID = inserted_ID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return _Driver_ID;

        }

        public static bool UpdateDriver(int _Driver_ID, int _Person_ID, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //we dont update the _CreatedDate for the driver.
            string query = @"Update  Drivers  
                            set PersonID = @_Person_ID,
                                CreatedByUserID= @_CreatedByUser_ID
                                where DriverID = @_Driver_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
           
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

    }
}
