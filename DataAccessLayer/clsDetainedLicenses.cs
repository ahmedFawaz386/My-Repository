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
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoBy_ID(int Detain_ID, 
            ref int _License_ID, ref DateTime DetainDate,
            ref float Fine_Fees,ref int _CreatedByUser_ID, 
            ref bool IsReleased, ref DateTime ReleaseDate, 
            ref int ReleasedByUser_ID,ref int Release_Application_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @Detain_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Detain_ID", Detain_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                    _License_ID = (int)reader["LicenseID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    Fine_Fees = Convert.ToSingle(reader["FineFees"]);
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];

                    IsReleased = (bool)reader["IsReleased"];

                    if(reader["ReleaseDate"] ==DBNull.Value ) 
                   
                        ReleaseDate = DateTime.MaxValue;
                    else
                        ReleaseDate = (DateTime)reader["ReleaseDate"];


                    if (reader["ReleasedByUserID"] == DBNull.Value)

                        ReleasedByUser_ID = -1;
                    else
                        ReleasedByUser_ID = (int)reader["ReleasedByUserID"];

                    if (reader["ReleaseApplicationID"] == DBNull.Value)

                        Release_Application_ID = -1;
                    else
                        Release_Application_ID = (int)reader["ReleaseApplicationID"];

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

        
        public static bool GetDetainedLicenseInfoBy_License_ID(int _License_ID,
         ref int Detain_ID, ref DateTime DetainDate,
         ref float Fine_Fees, ref int _CreatedByUser_ID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUser_ID, ref int Release_Application_ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID= @_License_ID order by DetainID desc";

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

                    _License_ID = (int)reader["LicenseID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    Fine_Fees = Convert.ToSingle(reader["FineFees"]);
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];

                    IsReleased = (bool)reader["IsReleased"];

                    if (reader["ReleaseDate"] == DBNull.Value)

                        ReleaseDate = DateTime.MaxValue;
                    else
                        ReleaseDate = (DateTime)reader["ReleaseDate"];


                    if (reader["ReleasedByUserID"] == DBNull.Value)

                        ReleasedByUser_ID = -1;
                    else
                        ReleasedByUser_ID = (int)reader["ReleasedByUserID"];

                    if (reader["ReleaseApplicationID"] == DBNull.Value)

                        Release_Application_ID = -1;
                    else
                        Release_Application_ID = (int)reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "select * from DetainedLicenses_View order by IsReleased ,Detain_ID;";

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

        public static int AddNewDetainedLicense(
            int _License_ID,  DateTime DetainDate,
            float Fine_Fees,  int _CreatedByUser_ID)
        {
            int Detain_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased
                               )
                            VALUES
                               (@_License_ID,
                               @DetainDate, 
                               @Fine_Fees, 
                               @_CreatedByUser_ID,
                               0
                             );
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_License_ID", _License_ID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@Fine_Fees", Fine_Fees);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
          
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    Detain_ID = inserted_ID;
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


            return Detain_ID;

        }

        public static bool UpdateDetainedLicense(int Detain_ID, 
            int _License_ID, DateTime DetainDate,
            float Fine_Fees, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                              SET LicenseID = @_License_ID, 
                              DetainDate = @DetainDate, 
                              FineFees = @Fine_Fees,
                              CreatedByUserID= @_CreatedByUser_ID,   
                              WHERE DetainID=@Detain_ID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Detained_License_ID", Detain_ID);
            command.Parameters.AddWithValue("@_License_ID", _License_ID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@Fine_Fees", Fine_Fees);
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


        public static bool ReleaseDetainedLicense(int Detain_ID,
                 int ReleasedByUser_ID, int Release_Application_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID= @Release_Application_ID   
                              WHERE DetainID=@Detain_ID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Detain_ID", Detain_ID);
            command.Parameters.AddWithValue("@ReleasedByUser_ID", ReleasedByUser_ID);
            command.Parameters.AddWithValue("@Release_Application_ID", Release_Application_ID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
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

        public static bool IsLicenseDetained(int _License_ID)
        {
            bool _IsDetained = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select _IsDetained=1 
                            from DetainedLicenses
                            where 
                            LicenseID=@_License_ID 
                            and IsReleased=0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_License_ID", _License_ID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null )
                {
                    _IsDetained = Convert.ToBoolean(result);
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


            return _IsDetained;
            ;

        }

    }
}
