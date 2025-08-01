using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {

        public static bool GetLicenseInfoBy_ID(int _License_ID,ref int _Application_ID, ref int _Driver_ID, ref int _LicenseClassID,
            ref DateTime _IssueDate, ref DateTime _ExpirationDate,ref string _Notes,
            ref float _Pa_ID_Fees,ref bool _IsActive, ref byte _IssueReason, ref int _CreatedByUser_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Licenses WHERE LicenseID = @_License_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@_License_ID", _License_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;
                        _Application_ID= (int)reader["ApplicationID"];
                        _Driver_ID  = (int)reader["DriverID"];
                        _LicenseClassID = (int)reader["LicenseClass"];
                        _IssueDate=(DateTime)reader["IssueDate"];
                        _ExpirationDate = (DateTime)reader["ExpirationDate"];

                        if (reader["Notes"] ==DBNull.Value)
                            _Notes = "";
                        else
                            _Notes = (string)reader["Notes"];

                        _Pa_ID_Fees = Convert.ToSingle(reader["PaidFees"]);
                        _IsActive = (bool)reader["IsActive"];
                        _IssueReason = (byte)reader["IssueReason"];
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
        public static bool Find_ByAppID(ref int _License_ID,int _Application_ID, ref int _Driver_ID, ref int _LicenseClassID,
            ref DateTime _IssueDate, ref DateTime _ExpirationDate,ref string _Notes,
            ref float _Pa_ID_Fees,ref bool _IsActive, ref byte _IssueReason, ref int _CreatedByUser_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationID", _Application_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;
                        _License_ID = (int)reader["LicenseID"];
                        _Driver_ID  = (int)reader["DriverID"];
                        _LicenseClassID = (int)reader["LicenseClass"];
                        _IssueDate=(DateTime)reader["IssueDate"];
                        _ExpirationDate = (DateTime)reader["ExpirationDate"];

                        if (reader["Notes"]==DBNull.Value)
                            _Notes = "";
                        else
                            _Notes = (string)reader["Notes"];

                        _Pa_ID_Fees = Convert.ToSingle(reader["PaidFees"]);
                        _IsActive = (bool)reader["IsActive"];
                        _IssueReason = (byte)reader["IssueReason"];
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

        public static DataTable GetAllLicenses()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Licenses";

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

        public static DataTable GetDriverLicenses(int _Driver_ID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses._ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID = @_Driver_ID
                            Order By IsActive Desc, ExpirationDate Desc";

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

        public static int AddNewLicense(  int _Application_ID, int _Driver_ID,  int _LicenseClassID,
             DateTime _IssueDate,  DateTime _ExpirationDate,  string _Notes,
             float _Pa_ID_Fees,  bool _IsActive,byte _IssueReason,  int _CreatedByUser_ID)
        {
            int _License_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                              INSERT INTO Licenses
                               (ApplicationID,
                                DriverID,
                                LicenseClass,
                                IssueDate,
                                ExpirationDate,
                                Notes,
                                PaidFees,
                                IsActive,_IssueReason,
                                CreatedByUserID)
                         VALUES
                               (
                               @_Application_ID,
                               @_Driver_ID,
                               @_LicenseClassID,
                               @_IssueDate,
                               @_ExpirationDate,
                               @_Notes,
                               @_Pa_ID_Fees,
                               @_IsActive,@_IssueReason, 
                               @_CreatedByUser_ID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            command.Parameters.AddWithValue("@_LicenseClassID", _LicenseClassID);
            command.Parameters.AddWithValue("@_IssueDate", _IssueDate);

            command.Parameters.AddWithValue("@_ExpirationDate", _ExpirationDate);

            if (_Notes == "")
                command.Parameters.AddWithValue("@_Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@_Notes", _Notes);

            command.Parameters.AddWithValue("@_Pa_ID_Fees", _Pa_ID_Fees);
            command.Parameters.AddWithValue("@_IsActive", _IsActive);
            command.Parameters.AddWithValue("@_IssueReason", _IssueReason);
     
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
           


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _License_ID = inserted_ID;
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


            return _License_ID;

        }

        public static bool UpdateLicense(int _License_ID ,int _Application_ID, int _Driver_ID, int _LicenseClassID,
             DateTime _IssueDate, DateTime _ExpirationDate, string _Notes,
             float _Pa_ID_Fees, bool _IsActive,byte _IssueReason, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Licenses
                           SET ApplicationID=@_Application_ID, DriverID = @_Driver_ID,
                              LicenseClass = @_LicenseClassID,
                              IssueDate = @_IssueDate,
                              ExpirationDate = @_ExpirationDate,
                              Notes = @_Notes,
                              PaidFees = @_Pa_ID_Fees,
                              IsActive = @_IsActive,IssueReason=@_IssueReason,
                              CreatedByUserID = @_CreatedByUser_ID
                         WHERE LicenseID=@_License_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_License_ID", _License_ID);
            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("@_Driver_ID", _Driver_ID);
            command.Parameters.AddWithValue("@_LicenseClassID", _LicenseClassID);
            command.Parameters.AddWithValue("@_IssueDate", _IssueDate);
            command.Parameters.AddWithValue("@_ExpirationDate", _ExpirationDate);
            
            if (_Notes=="")
                command.Parameters.AddWithValue("@_Notes", DBNull.Value );
            else
                command.Parameters.AddWithValue("@_Notes", _Notes);

            command.Parameters.AddWithValue("@_Pa_ID_Fees", _Pa_ID_Fees);
            command.Parameters.AddWithValue("@_IsActive", _IsActive);
            command.Parameters.AddWithValue("@_IssueReason", _IssueReason);
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

        public static int GetActive_License_IDBy_Person_ID(int _Person_ID,int _LicenseClassID_ID)
        {
            int _License_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT        Licenses._License_ID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses._Driver_ID = Drivers._Driver_ID
                            WHERE  
                             
                             Licenses._LicenseClassID = @_LicenseClassID 
                              AND Drivers._Person_ID = @_Person_ID
                              And _IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_LicenseClassID", _LicenseClassID_ID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _License_ID = inserted_ID;
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


            return _License_ID;
        }

        public static bool DeactivateLicense(int _License_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Licenses
                           SET 
                              IsActive = 0
                             
                         WHERE LicenseID=@_License_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_License_ID", _License_ID);
         

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
