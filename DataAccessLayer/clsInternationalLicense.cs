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
using System.ComponentModel;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {

        public static bool GetInternationalLicenseInfoBy_ID(int International_License_ID, 
            ref int _Application_ID, 
            ref int _Driver_ID, ref int IssuedUsingLocal_License_ID, 
            ref DateTime _IssueDate, ref DateTime _ExpirationDate,ref bool _IsActive, ref int _CreatedByUser_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @International_License_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@International_License_ID", International_License_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;
                        _Application_ID = (int)reader["ApplicationID"];
                        _Driver_ID  = (int)reader["DriverID"];
                        IssuedUsingLocal_License_ID = (int)reader["IssuedUsingLocalLicenseID"];
                        _IssueDate=(DateTime)reader["IssueDate"];
                        _ExpirationDate = (DateTime)reader["ExpirationDate"];

                       
                        _IsActive = (bool)reader["IsActive"];
                        _CreatedByUser_ID = (int)reader["CreatedByUserID"];


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

         public static DataTable GetAllInternationalLicenses()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
            SELECT    InternationalLicenseID, ApplicationID,DriverID,
		                IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
		    from InternationalLicenses 
                order by IsActive, ExpirationDate desc";

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

         public static DataTable GetDriverInternationalLicenses(int _Driver_ID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
            SELECT    InternationalLicenseID, ApplicationID,
		                IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
		    from InternationalLicenses where DriverID=@_Driver_ID
                order by ExpirationDate desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);

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


        public static int AddNewInternationalLicense( int _Application_ID,
             int _Driver_ID,  int IssuedUsingLocal_License_ID,
             DateTime _IssueDate,  DateTime _ExpirationDate, bool _IsActive,  int _CreatedByUser_ID)
        {
            int International_License_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                               Update InternationalLicenses
                               set IsActive=0
                               where DriverID=@_Driver_ID;

                             INSERT INTO InternationalLicenses
                               (
                                ApplicationID,
                                DriverID,
                                IssuedUsingLocalLicenseID,
                                IssueDate,
                                ExpirationDate,
                                IsActive,
                                CreatedByUserID)
                         VALUES
                               (@_Application_ID,
                                @_Driver_ID,
                                @IssuedUsingLocal_License_ID,
                                @_IssueDate,
                                @_ExpirationDate,
                                @_IsActive,
                                @_CreatedByUser_ID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            command.Parameters.AddWithValue("@IssuedUsingLocal_License_ID", IssuedUsingLocal_License_ID);
            command.Parameters.AddWithValue("@_IssueDate", _IssueDate);
            command.Parameters.AddWithValue("@_ExpirationDate", _ExpirationDate);

            command.Parameters.AddWithValue("@_IsActive", _IsActive);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
           


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    International_License_ID = inserted_ID;
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


            return International_License_ID;

        }

        public static bool UpdateInternationalLicense(
              int International_License_ID , int _Application_ID,
             int _Driver_ID, int IssuedUsingLocal_License_ID,
             DateTime _IssueDate, DateTime _ExpirationDate, bool _IsActive, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses
                           SET 
                              ApplicationID=@_Application_ID,
                              DriverID = @_Driver_ID,
                              IssuedUsingLocalLicenseID = @IssuedUsingLocal_License_ID,
                              IssueDate = @_IssueDate,
                              ExpirationDate = @_ExpirationDate,
                              IsActive = @_IsActive,
                              CreatedByUserID = @_CreatedByUser_ID
                         WHERE InternationalLicenseID = @International_License_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@International_License_ID", International_License_ID);
            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            command.Parameters.AddWithValue("@IssuedUsingLocal_License_ID", IssuedUsingLocal_License_ID);
            command.Parameters.AddWithValue("@_IssueDate", _IssueDate);
            command.Parameters.AddWithValue("@_ExpirationDate", _ExpirationDate);
            
            command.Parameters.AddWithValue("@_IsActive", _IsActive);
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
        public static bool IsTherePreviousLicense(int _Driver_ID)
        {

            bool isthere = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select found = 1 from InternationalLicenses
                            where InternationalLicenses.IsActive = 1 and InternationalLicenses.DriverID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            
            try
            {
                connection.Open();
                object x = command.ExecuteScalar();
                isthere = (x!=null)?true:false;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                
            }

            finally
            {
                connection.Close();
            }

            return (isthere);
        }

        public static int GetActiveInternational_License_IDBy_Driver_ID(int _Driver_ID)
        {
            int International_License_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            //string query = @"  
            //                SELECT Top 1 InternationalLicenseID
            //                FROM InternationalLicenses 
            //                where DriverID=@_Driver_ID and GetDate() between IssueDate and ExpirationDate
            //                order by ExpirationDate Desc;";

            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@_Driver_ID 
                            order by ExpirationDate Desc;";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
          
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    International_License_ID = inserted_ID;
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


            return International_License_ID;
        }

    }
}
