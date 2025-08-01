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
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentInfoBy_ID(int TestAppointment_ID, 
            ref int TestType_ID, ref int _LocalDrivingLicense_Application_ID,
            ref DateTime  AppointmentDate, ref float _Pa_ID_Fees, ref int _CreatedByUser_ID, ref bool IsLocked, ref int RetakeTest_Application_ID)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointment_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestAppointment_ID", TestAppointment_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;
                    TestType_ID = (int)reader["TestTypeID"];
                    _LocalDrivingLicense_Application_ID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];
                    _Pa_ID_Fees = Convert.ToSingle( reader["PaidFees"]);
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] ==DBNull.Value)
                         RetakeTest_Application_ID = -1;
                    else
                        RetakeTest_Application_ID = (int)reader["RetakeTestApplicationID"];

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

        public static bool GetLastTestAppointment( 
             int _LocalDrivingLicense_Application_ID,  int TestType_ID, 
            ref int TestAppointment_ID,ref DateTime AppointmentDate,
            ref float _Pa_ID_Fees, ref int _CreatedByUser_ID,ref bool IsLocked,ref int RetakeTest_Application_ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT       top 1 *
                FROM            TestAppointments
                WHERE        (TestTypeID = @TestType_ID) 
                AND (LocalDrivingLicenseApplicationID = @_LocalDrivingLicense_Application_ID) 
                order by TestAppointmentID Desc";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_LocalDrivingLicense_Application_ID", _LocalDrivingLicense_Application_ID);
            command.Parameters.AddWithValue("@TestType_ID", TestType_ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    TestAppointment_ID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    _Pa_ID_Fees = Convert.ToSingle(reader["PaidFees"]);
                    _CreatedByUser_ID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTest_Application_ID = -1;
                    else
                        RetakeTest_Application_ID = (int)reader["RetakeTestApplicationID"];


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

        public static DataTable GetAllTestAppointments()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                 string query = @"select * from TestAppointments_View
                                  order by AppointmentDate Desc";


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

        public static DataTable GetApplicationTestAppointmentsPerTestType(int _LocalDrivingLicense_Application_ID,int TestType_ID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                        FROM TestAppointments
                        WHERE  
                        (TestTypeID = @TestType_ID) 
                        AND (LocalDrivingLicenseApplicationID = @_LocalDrivingLicense_Application_ID)
                        order by TestAppointmentID desc;";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_LocalDrivingLicense_Application_ID", _LocalDrivingLicense_Application_ID);
            command.Parameters.AddWithValue("@TestType_ID", TestType_ID);


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

        public static int AddNewTestAppointment(
             int TestType_ID,  int _LocalDrivingLicense_Application_ID,
             DateTime AppointmentDate,  float _Pa_ID_Fees,  int _CreatedByUser_ID,int RetakeTest_Application_ID)
        {
            int TestAppointment_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            Values (@TestType_ID,@_LocalDrivingLicense_Application_ID,@AppointmentDate,@_Pa_ID_Fees,@_CreatedByUser_ID,0,@RetakeTest_Application_ID);
                
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

           
            command.Parameters.AddWithValue("@TestType_ID", TestType_ID);
            command.Parameters.AddWithValue("@_LocalDrivingLicense_Application_ID", _LocalDrivingLicense_Application_ID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@_Pa_ID_Fees", _Pa_ID_Fees);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);

            if (RetakeTest_Application_ID == -1)

                command.Parameters.AddWithValue("@RetakeTest_Application_ID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTest_Application_ID", RetakeTest_Application_ID);





            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    TestAppointment_ID = inserted_ID;
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


            return TestAppointment_ID;

        }

        public static bool UpdateTestAppointment(int TestAppointment_ID,  int TestType_ID,  int _LocalDrivingLicense_Application_ID,
             DateTime AppointmentDate,  float _Pa_ID_Fees, 
             int _CreatedByUser_ID,bool IsLocked,int RetakeTest_Application_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  TestAppointments  
                            set TestTypeID = @TestType_ID,
                                LocalDrivingLicenseApplicationID = @_LocalDrivingLicense_Application_ID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @_Pa_ID_Fees,
                                CreatedByUserID= @_CreatedByUser_ID,
                                IsLocked=@IsLocked,
                                RetakeTestApplicationID=@RetakeTest_Application_ID
                                where TestAppointmentID = @TestAppointment_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointment_ID", TestAppointment_ID);
            command.Parameters.AddWithValue("@TestType_ID", TestType_ID);
            command.Parameters.AddWithValue("@_LocalDrivingLicense_Application_ID", _LocalDrivingLicense_Application_ID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@_Pa_ID_Fees", _Pa_ID_Fees);
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTest_Application_ID==-1)
            
                command.Parameters.AddWithValue("@RetakeTest_Application_ID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTest_Application_ID", RetakeTest_Application_ID);





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

        //****
        public static int GetTest_ID(int TestAppointment_ID)
        {
            int Test_ID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select TestTypeID from TestAppointments where TestAppointmentID=@TestAppointment_ID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointment_ID", TestAppointment_ID);
            

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    Test_ID = inserted_ID;
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


            return Test_ID;

        }
        public static int GetTrail(int TestType_ID,int locatLicense_id)
        {
            int Trail = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select count(TestAppointments.TestTypeID) from TestAppointments
                            where TestAppointments.LocalDrivingLicenseApplicationID = @L_id and TestAppointments.TestTypeID=@T_id";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@L_id", locatLicense_id);
            command.Parameters.AddWithValue("@T_id", TestType_ID);
            

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    Trail = inserted_ID;
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


            return Trail;

        }

    }
}
