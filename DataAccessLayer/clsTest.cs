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
    public class clsTestData
    {

        public static bool GetTestInfoBy_ID(int Test_ID, 
            ref int TestAppointment_ID,ref bool TestResult, 
            ref string _Notes , ref int _CreatedByUser_ID )
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Tests WHERE TestID = @Test_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Test_ID", Test_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                    TestAppointment_ID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] ==DBNull.Value)
                   
                        _Notes = "";
                    else
                        _Notes = (string)reader["Notes"];

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


        public static bool GetLastTestByPersonAndTestTypeAnd_LicenseClassID
            (int _Person_ID,int _LicenseClassID_ID,int TestType_ID, ref int Test_ID,
              ref int TestAppointment_ID, ref bool TestResult,
              ref string _Notes, ref int _CreatedByUser_ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT  top 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @_Person_ID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @_LicenseClassID_ID)
                        AND ( TestAppointments.TestTypeID=@TestType_ID)
                ORDER BY Tests.TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_LicenseClassID_ID", _LicenseClassID_ID);
            command.Parameters.AddWithValue("@TestType_ID", TestType_ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;
                    Test_ID = (int)reader["Test_ID"];
                    TestAppointment_ID = (int)reader["TestAppointment_ID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["_Notes"] == DBNull.Value)

                        _Notes = "";
                    else
                        _Notes = (string)reader["_Notes"];

                    _CreatedByUser_ID = (int)reader["_CreatedByUser_ID"];

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


        public static DataTable GetAllTests()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Tests order by TestID";

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

        public static int AddNewTest( int TestAppointment_ID,  bool TestResult,
             string _Notes,  int _CreatedByUser_ID)
        {
            int Test_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            //string query = @"Insert Into Tests (TestAppointmentID,TestResult,
            //                                    Notes,   CreatedByUserID)
            //                Values (@TestAppointment_ID,@TestResult,
            //                                    @_Notes,   @_CreatedByUser_ID);
                            
            //                    UPDATE TestAppointments 
            //                    SET IsLocked=1 where TestAppointmentID = @TestAppointment_ID;

            //                    SELECT SCOPE__IDENTITY();";

            string query = @"Insert Into Tests (TestAppointmentID,TestResult,
                                                Notes,   CreatedByUserID)
                            Values (@TestAppointment_ID,@TestResult,
                                                @_Notes,   @_CreatedByUser_ID);
                            
                               

                                SELECT SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointment_ID", TestAppointment_ID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (_Notes != "" && _Notes != null)
                command.Parameters.AddWithValue("@_Notes", _Notes);
            else
                command.Parameters.AddWithValue("@_Notes", System.DBNull.Value);

      
            
            command.Parameters.AddWithValue("@_CreatedByUser_ID", _CreatedByUser_ID);

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

        public static bool UpdateTest(int Test_ID, int TestAppointment_ID, bool TestResult,
             string _Notes, int _CreatedByUser_ID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Tests  
                            set TestAppointmentID = @TestAppointment_ID,
                                TestResult=@TestResult,
                                Notes = @_Notes,
                                CreatedByUserID =@_CreatedByUser_ID
                                where TestID = @Test_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Test_ID", Test_ID);
            command.Parameters.AddWithValue("@TestAppointment_ID", TestAppointment_ID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@_Notes", _Notes);
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
        public static bool CheckTestResult(int Test_ID, int Licence_ID)
        {

            bool Result = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select top 1 (Tests.TestResult) from TestAppointments
                             inner join Tests on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             where TestAppointments.TestTypeID=@Typ_id and TestAppointments.LocalDrivingLicenseApplicationID = @L_id
                             order by Tests.TestID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Typ_id", Test_ID);
            command.Parameters.AddWithValue("@L_id", Licence_ID);
            
            try
            {
                object x = command.ExecuteScalar();
                connection.Open();
                
                Result = Convert.ToBoolean(x.ToString());

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

            return Result;
        }

        public static byte GetPassedTestCount(int _LocalDrivingLicense_Application_ID)
        {
            byte PassedTestCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 where LocalDrivingLicenseApplicationID =@_LocalDrivingLicense_Application_ID and TestResult=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_LocalDrivingLicense_Application_ID", _LocalDrivingLicense_Application_ID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
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

            return PassedTestCount;



        }



    }
}
