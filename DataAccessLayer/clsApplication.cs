using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {
      

        public static bool GetApplicationInfoBy_ID(int _Application_ID, 
            ref int _Applicant_Person_ID, ref DateTime _ApplicationDate, ref int _ApplicationType_ID, 
            ref byte _ApplicationStatus,ref DateTime _LastStatusDate,
            ref float _Pa_ID_Fees, ref int _CreatedByUser_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Applications WHERE ApplicationID = @_Application_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@_Application_ID", _Application_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                        _Applicant_Person_ID = (int)reader["ApplicantPersonID"];
                        _ApplicationDate = (DateTime) reader["ApplicationDate"];
                        _ApplicationType_ID = (int)reader["ApplicationTypeID"];
                        _ApplicationStatus = (byte)reader["ApplicationStatus"];
                        _LastStatusDate = (DateTime)reader["ApplicationStatus"];
                        _Pa_ID_Fees = Convert.ToSingle(reader["PaidFees"]);
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

        public static DataTable GetAllApplications()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                //string query = "select * from ApplicationsList_View order by _ApplicationDate desc";
                string query = "select * from Applications order by _ApplicationDate desc";

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

        public static int AddNewApplication( int _Applicant_Person_ID,  DateTime _ApplicationDate,  int _ApplicationType_ID,
             byte _ApplicationStatus,  DateTime _LastStatusDate,
             float _Pa_ID_Fees,  int _CreatedByUser_ID)
        {

            //this function will return the new person _ID if succeeded and -1 if not.
            int _Application_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications ( 
                            ApplicantPersonID,ApplicationDate,ApplicationTypeID,
                            ApplicationStatus,LastStatusDate,
                            PaidFees,CreatedByUserID)
                             VALUES (@_Applicant_Person_ID,@_ApplicationDate,@_ApplicationType_ID,
                                      @_ApplicationStatus,@_LastStatusDate,
                                      @_Pa_ID_Fees,   @_CreatedByUser_ID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("_Applicant_Person_ID", @_Applicant_Person_ID);
            command.Parameters.AddWithValue("_ApplicationDate", @_ApplicationDate);
            command.Parameters.AddWithValue("_ApplicationType_ID", @_ApplicationType_ID);
            command.Parameters.AddWithValue("_ApplicationStatus", @_ApplicationStatus);
            command.Parameters.AddWithValue("_LastStatusDate", @_LastStatusDate);
            command.Parameters.AddWithValue("_Pa_ID_Fees", @_Pa_ID_Fees);
            command.Parameters.AddWithValue("_CreatedByUser_ID", @_CreatedByUser_ID);




            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _Application_ID = inserted_ID;
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


            return _Application_ID;
        }


        public static bool UpdateApplication(int _Application_ID, int _Applicant_Person_ID, DateTime _ApplicationDate, int _ApplicationType_ID,
             byte _ApplicationStatus, DateTime _LastStatusDate,
             float _Pa_ID_Fees, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Applications  
                            set ApplicationID = @_Applicant_Person_ID,
                                ApplicationDate = @_ApplicationDate,
                                ApplicationTypeID = @_ApplicationType_ID,
                                ApplicationStatus = @_ApplicationStatus, 
                                LastStatusDate = @_LastStatusDate,
                                PaidFees = @_Pa_ID_Fees,
                                CreatedByUserID=@_CreatedByUser_ID
                            where ApplicationID=@_Application_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("_Applicant_Person_ID", @_Applicant_Person_ID);
            command.Parameters.AddWithValue("_ApplicationDate", @_ApplicationDate);
            command.Parameters.AddWithValue("_ApplicationType_ID", @_ApplicationType_ID);
            command.Parameters.AddWithValue("_ApplicationStatus", @_ApplicationStatus);
            command.Parameters.AddWithValue("_LastStatusDate", @_LastStatusDate);
            command.Parameters.AddWithValue("_Pa_ID_Fees", @_Pa_ID_Fees);
            command.Parameters.AddWithValue("_CreatedByUser_ID", @_CreatedByUser_ID);


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

        public static bool DeleteApplication(int _Application_ID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Applications 
                                where ApplicationID = @_Application_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsApplicationExist(int _Application_ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @_Application_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

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

        public static bool DoesPersonHaveActiveApplication(int _Person_ID, int _ApplicationType_ID)
        {
           
           //incase the ActiveApplication _ID !=-1 return true.
            return (GetActive_Application_ID(_Person_ID, _ApplicationType_ID) !=-1);
        }

        public static int GetActive_Application_ID(int _Person_ID, int _ApplicationType_ID)
        {
            int Active_Application_ID =-1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Active_Application_ID=ApplicationID FROM Applications WHERE ApplicantPersonID = @_Applicant_Person_ID and ApplicationTypeID=@_ApplicationType_ID and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Applicant_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_ApplicationType_ID", _ApplicationType_ID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
             

                if (result != null && int.TryParse(result.ToString(), out int App_ID))
                {
                    Active_Application_ID = App_ID;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return Active_Application_ID;
            }
            finally
            {
                connection.Close();
            }

            return Active_Application_ID;
        }

        public static int GetActive_Application_IDFor_LicenseClassID(int _Person_ID, int _ApplicationType_ID,int _LicenseClassID_ID)
        {
            int Active_Application_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Active_Application_ID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @_Applicant_Person_ID 
                            and ApplicationTypeID=@_ApplicationType_ID 
							and LocalDrivingLicenseApplications.LicenseClassID = @_LicenseClassID_ID
                            and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Applicant_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_ApplicationType_ID", _ApplicationType_ID);
            command.Parameters.AddWithValue("@_LicenseClassID_ID", _LicenseClassID_ID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int App_ID))
                {
                    Active_Application_ID = App_ID;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return Active_Application_ID;
            }
            finally
            {
                connection.Close();
            }

            return Active_Application_ID;
        }
      
        public static bool UpdateStatus(int _Application_ID, short NewStatus)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @_LastStatusDate
                            where ApplicationID=@_Application_ID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Application_ID", _Application_ID);
            command.Parameters.AddWithValue("@NewStatus", NewStatus);
            command.Parameters.AddWithValue("_LastStatusDate", DateTime.Now);
            

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
