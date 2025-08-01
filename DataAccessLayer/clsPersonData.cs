using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
       
        public static bool GetPersonInfoBy_ID(int _Person_ID, ref string _FirstName, ref string _SecondName,
          ref string _ThirdName, ref string _LastName, ref string _NationalNo, ref DateTime _DateOfBirth,
           ref short _Gendor,ref string _Address,  ref string _Phone, ref string _Email,
           ref int _NationalityCountry_ID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @_Person_ID";

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

                    _FirstName = (string)reader["FirstName"];
                    _SecondName = (string)reader["SecondName"];

                    //_ThirdName: allows null in database so we should handle null
                    if (reader["_ThirdName"] != DBNull.Value)
                    {
                        _ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        _ThirdName = "";
                    }

                    _LastName = (string)reader["LastName"];
                    _NationalNo = (string)reader["NationalNo"];
                    _DateOfBirth = (DateTime)reader["DateOfBirth"];
                    _Gendor = (byte) reader["Gendor"];
                    _Address = (string)reader["Address"];
                    _Phone = (string)reader["Phone"];


                    //_Email: allows null in database so we should handle null
                    if (reader["_Email"] != DBNull.Value)
                    {
                        _Email = (string)reader["Email"];
                    }
                    else
                    {
                        _Email = "";
                    }

                    _NationalityCountry_ID = (int)reader["_NationalityCountry_ID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

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


        public static bool GetPersonInfoBy_NationalNo(string _NationalNo, ref int _Person_ID, ref string _FirstName, ref string _SecondName,
        ref string _ThirdName, ref string _LastName,   ref DateTime _DateOfBirth,
         ref short _Gendor,ref string _Address, ref string _Phone, ref string _Email,
         ref int _NationalityCountry_ID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @_NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_NationalNo", _NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    _Person_ID = (int)reader["PersonID"];
                    _FirstName = (string)reader["FirstName"];
                    _SecondName = (string)reader["SecondName"];

                    //_ThirdName: allows null in database so we should handle null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        _ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        _ThirdName = "";
                    }

                    _LastName = (string)reader["LastName"];
                    _DateOfBirth = (DateTime)reader["DateOfBirth"];
                    _Gendor = (byte)reader["Gendor"];
                    _Address = (string)reader["Address"];
                    _Phone = (string)reader["Phone"];

                    //_Email: allows null in database so we should handle null
                    if (reader["Email"] != DBNull.Value)
                    {
                        _Email = (string)reader["Email"];
                    }
                    else
                    {
                        _Email = "";
                    }

                    _NationalityCountry_ID = (int)reader["_NationalityCountry_ID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

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



        public static int AddNewPerson( string _FirstName,  string _SecondName,
           string _ThirdName,  string _LastName,  string _NationalNo,  DateTime _DateOfBirth,
           short _Gendor, string _Address,  string _Phone,  string _Email,
            int _NationalityCountry_ID,  string ImagePath)
        {
            //this function will return the new person _ID if succeeded and -1 if not.
            int _Person_ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                             VALUES (@_FirstName, @_SecondName,@_ThirdName, @_LastName, @_NationalNo,
                                     @_DateOfBirth,@_Gendor,@_Address,@_Phone, @_Email,@_NationalityCountry_ID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_FirstName", _FirstName);
            command.Parameters.AddWithValue("@_SecondName", _SecondName);
           
            if (_ThirdName != "" && _ThirdName != null)
                command.Parameters.AddWithValue("@_ThirdName", _ThirdName);
            else
                command.Parameters.AddWithValue("@_ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@_LastName", _LastName);
            command.Parameters.AddWithValue("@_NationalNo", _NationalNo);
            command.Parameters.AddWithValue("@_DateOfBirth", _DateOfBirth);
            command.Parameters.AddWithValue("@_Gendor", _Gendor);
            command.Parameters.AddWithValue("@_Address", _Address);
            command.Parameters.AddWithValue("@_Phone", _Phone);
            
            if (_Email != "" && _Email != null)
                command.Parameters.AddWithValue("@_Email", _Email);
            else
                command.Parameters.AddWithValue("@_Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@_NationalityCountry_ID", _NationalityCountry_ID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int inserted_ID))
                {
                    _Person_ID = inserted_ID;
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

            return _Person_ID;
        }



        public static bool UpdatePerson(int _Person_ID,  string _FirstName, string _SecondName,
           string _ThirdName, string _LastName, string _NationalNo, DateTime _DateOfBirth,
           short _Gendor, string _Address, string _Phone, string _Email,
            int _NationalityCountry_ID, string ImagePath)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  People  
                            set FirstName = @_FirstName,
                                SecondName = @_SecondName,
                                ThirdName = @_ThirdName,
                                LastName = @_LastName, 
                                NationalNo = @_NationalNo,
                                DateOfBirth = @_DateOfBirth,
                                Gendor=@_Gendor,
                                Address = @_Address,  
                                Phone = @_Phone,
                                Email = @_Email, 
                                NationalityCountry_ID = @_NationalityCountry_ID,
                                ImagePath =@ImagePath
                                where PersonID = @_Person_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);
            command.Parameters.AddWithValue("@_FirstName", _FirstName);
            command.Parameters.AddWithValue("@_SecondName", _SecondName);

            if (_ThirdName != "" && _ThirdName != null)
                command.Parameters.AddWithValue("@_ThirdName", _ThirdName);
            else
                command.Parameters.AddWithValue("@_ThirdName", System.DBNull.Value);

          
            command.Parameters.AddWithValue("@_LastName", _LastName);
            command.Parameters.AddWithValue("@_NationalNo", _NationalNo);
            command.Parameters.AddWithValue("@_DateOfBirth", _DateOfBirth);
            command.Parameters.AddWithValue("@_Gendor", _Gendor);
            command.Parameters.AddWithValue("@_Address", _Address);
            command.Parameters.AddWithValue("@_Phone", _Phone);

            if (_Email != "" && _Email != null)
                command.Parameters.AddWithValue("@_Email", _Email);
            else
                command.Parameters.AddWithValue("@_Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@_NationalityCountry_ID", _NationalityCountry_ID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


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


        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'

                  ELSE 'Female'

                  END as _GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";




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

        public static bool DeletePerson(int _Person_ID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete People 
                                where PersonID = @_Person_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);

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

        public static bool IsPersonExist(int _Person_ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @_Person_ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_Person_ID", _Person_ID);

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

        public static bool IsPersonExist(string _NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @_NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@_NationalNo", _NationalNo);

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


    }
}
