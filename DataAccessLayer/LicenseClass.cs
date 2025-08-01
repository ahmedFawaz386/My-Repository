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
    public class clsLicenseClassData
    {

        public static bool Get_LicenseClassIDInfoBy_ID(int _LicenseClassID_ID, 
            ref string _ClassName, ref string _ClassDescription, ref byte _MinimumAllowedAge, 
            ref byte _DefaultVal_IDityLength, ref float _Class_Fees)
            {
                bool isFound = false;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @_LicenseClassID_ID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@_LicenseClassID_ID", _LicenseClassID_ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                        _ClassName= (string)reader["ClassName"];
                        _ClassDescription = (string)reader["ClassDescription"];
                        _MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                        _DefaultVal_IDityLength = (byte) reader["DefaultValidityLength"];
                        _Class_Fees = Convert.ToSingle(reader["ClassFees"]);

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


        public static bool Get_LicenseClassIDInfoBy_ClassName( string _ClassName, ref int _LicenseClassID_ID,
            ref string _ClassDescription, ref byte _MinimumAllowedAge,
           ref byte _DefaultVal_IDityLength, ref float _Class_Fees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @_ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_ClassName", _ClassName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    _LicenseClassID_ID = (int)reader["LicenseClassID"];
                    _ClassDescription = (string)reader["ClassDescription"];
                    _MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    _DefaultVal_IDityLength = (byte)reader["DefaultValidityLength"];
                    _Class_Fees = Convert.ToSingle(reader["ClassFees"]);

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



        public static DataTable GetAll_LicenseClassIDes()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM LicenseClasses order by ClassName";

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

        public static int AddNew_LicenseClassID(string _ClassName, string _ClassDescription,
            byte _MinimumAllowedAge,byte _DefaultVal_IDityLength, float _Class_Fees)
        {
            int _LicenseClassID_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Insert Into LicenseClasses 
           (
            ClassName,ClassDescription,MinimumAllowedAge, 
            DefaultValidityLength,ClassFees)
                            Values ( 
            @_ClassName,@_ClassDescription,@_MinimumAllowedAge, 
            @_DefaultVal_IDityLength,@_Class_Fees)
                            
                            SELECT SCOPE_IDENTITY();";
            
          

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_ClassName", _ClassName);
            command.Parameters.AddWithValue("@_ClassDescription", _ClassDescription);
            command.Parameters.AddWithValue("@_MinimumAllowedAge", _MinimumAllowedAge);
            command.Parameters.AddWithValue("@_DefaultVal_IDityLength", _DefaultVal_IDityLength);
            command.Parameters.AddWithValue("@_Class_Fees", _Class_Fees);
 


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _LicenseClassID_ID = inserted_ID;
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


            return _LicenseClassID_ID;

        }

        public static bool Update_LicenseClassID(int _LicenseClassID_ID, string _ClassName, 
            string _ClassDescription,
            byte _MinimumAllowedAge, byte _DefaultVal_IDityLength, float _Class_Fees)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  LicenseClasses
                            set ClassName = @_ClassName,
                                ClassDescription = @_ClassDescription,
                                MinimumAllowedAge = @_MinimumAllowedAge,
                                DefaultValidityLength = @_DefaultVal_IDityLength,
                                ClassFees = @_Class_Fees
                                where LicenseClassID = @_LicenseClassID_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_LicenseClassID_ID", _LicenseClassID_ID);
            command.Parameters.AddWithValue("@_ClassName", _ClassName);
            command.Parameters.AddWithValue("@_ClassDescription", _ClassDescription);
            command.Parameters.AddWithValue("@_MinimumAllowedAge", _MinimumAllowedAge);
            command.Parameters.AddWithValue("@_DefaultVal_IDityLength", _DefaultVal_IDityLength);
            command.Parameters.AddWithValue("@_Class_Fees", _Class_Fees);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                //return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


    }
}
