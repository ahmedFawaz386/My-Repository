using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsTests
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int _TestID { set; get; }
        public int _TestAppointmentID { set; get; }
        public clsTestAppointments TestAppointmentInfo { set; get; }
        public bool _TestResult { set; get; }
        public string _Notes { set; get; }
        public int _CreatedByUserID { set; get; } 
       
        public clsTests()

        {
            this._TestID = -1;
            this._TestAppointmentID = -1;
            this._TestResult = false;
            this._Notes ="";
            this._CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        public clsTests(int Test_ID,int TestAppointment_ID,
            bool TestResult, string _Notes, int _CreatedByUser_ID)

        {
            this._TestID = Test_ID;
            this._TestAppointmentID = TestAppointment_ID;
            this.TestAppointmentInfo = clsTestAppointments.Find_ByID(TestAppointment_ID);
            this._TestResult = TestResult;
            this._Notes = _Notes;
            this._CreatedByUserID = _CreatedByUser_ID;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this._TestID = clsTestData.AddNewTest(this._TestAppointmentID,
                this._TestResult,this._Notes,this._CreatedByUserID);
              

            return (this._TestID != -1);
        }

        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestData.UpdateTest(this._TestID, this._TestAppointmentID,
                this._TestResult, this._Notes, this._CreatedByUserID);
        }

        public static clsTests Find(int Test_ID)
        {
            int TestAppointment_ID = -1;
            bool TestResult = false; string _Notes = "";int _CreatedByUser_ID = -1;

            if (clsTestData.GetTestInfoBy_ID( Test_ID,
            ref  TestAppointment_ID, ref  TestResult,
            ref  _Notes, ref  _CreatedByUser_ID))

                return new clsTests(Test_ID,
                        TestAppointment_ID,  TestResult,
                        _Notes,  _CreatedByUser_ID);
            else
                return null;

        }

        public static clsTests FindLastTestPerPersonAnd_LicenseClassID
            (int _Person_ID, int _LicenseClassID_ID, clsTestTypes.enTestType TestType_ID)
        {
            int Test_ID = -1;
            int TestAppointment_ID = -1;
            bool TestResult = false; string _Notes = ""; int _CreatedByUser_ID = -1;

            if (clsTestData.GetLastTestByPersonAndTestTypeAnd_LicenseClassID
                (_Person_ID,_LicenseClassID_ID,(int) TestType_ID, ref Test_ID,
            ref TestAppointment_ID, ref TestResult,
            ref _Notes, ref _CreatedByUser_ID))

                return new clsTests(Test_ID,
                        TestAppointment_ID, TestResult,
                        _Notes, _CreatedByUser_ID);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        public static byte GetPassedTests(int _LocalDrivingLicense_Application_ID)
        {
            return clsTestData.GetPassedTestCount(_LocalDrivingLicense_Application_ID);
        }

        public static bool  PassedAllTests(int _LocalDrivingLicense_Application_ID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTests(_LocalDrivingLicense_Application_ID) == 3;
        }
        public static bool CheckTestResult(int typ_id,int _LocalDrivingLicense_Application_ID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return CheckTestResult(typ_id,_LocalDrivingLicense_Application_ID);
        }

    }
}
