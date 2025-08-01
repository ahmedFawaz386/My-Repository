using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD_DataAccess;

namespace Buisness
{
    public class clsTestAppointments
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointment_ID { set; get; }
        public clsTestTypes.enTestType TestType_ID { set; get; }
        public int _LocalDrivingLicenseApplicationID { set; get; }
        public DateTime _AppointmentDate { set; get; }
        public float _PaidFees { set; get; }
        public int _CreatedByUserID { set; get; }
        public bool _IsLocked { set; get; }
        public int _RetakeTestApplicationID { set; get; }
        public clsApplications RetakeTestAppInfo { set; get; }

        public int  _TestTypeID   
        {
            get { return _GetTest_ID(); }   
          
        }

        public clsTestAppointments()

        {
            this.TestAppointment_ID = -1;
            this.TestType_ID = clsTestTypes.enTestType.VisionTest;
            this._AppointmentDate = DateTime.Now;
            this._PaidFees = 0;
            this._CreatedByUserID = -1;
            this._RetakeTestApplicationID = -1;  
            Mode = enMode.AddNew;

        }

        public clsTestAppointments(int TestAppointment_ID, clsTestTypes.enTestType TestType_ID,
           int _LocalDrivingLicense_Application_ID, DateTime AppointmentDate, float _Pa_ID_Fees, 
           int _CreatedByUser_ID ,bool IsLocked, int RetakeTest_Application_ID)

        {
            this.TestAppointment_ID = TestAppointment_ID;
            this.TestType_ID = TestType_ID;
            this._LocalDrivingLicenseApplicationID = _LocalDrivingLicense_Application_ID;
            this._AppointmentDate = AppointmentDate;
            this._PaidFees = _Pa_ID_Fees;
            this._CreatedByUserID = _CreatedByUser_ID;
            this._IsLocked = IsLocked;
            this._RetakeTestApplicationID= RetakeTest_Application_ID;
            this.RetakeTestAppInfo = clsApplications.FindBaseApplication(RetakeTest_Application_ID);
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            

            this.TestAppointment_ID = clsTestAppointmentData.AddNewTestAppointment((int) this.TestType_ID,this._LocalDrivingLicenseApplicationID,
                this._AppointmentDate,this._PaidFees,this._CreatedByUserID,this._RetakeTestApplicationID);

            return (this.TestAppointment_ID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            

            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointment_ID, (int) this.TestType_ID, this._LocalDrivingLicenseApplicationID,
                this._AppointmentDate, this._PaidFees, this._CreatedByUserID,this._IsLocked,this._RetakeTestApplicationID);
        }

        public static clsTestAppointments Find_ByID(int TestAppointment_ID)
        {
            int TestType_ID = 1;  int _LocalDrivingLicense_Application_ID = -1;
            DateTime AppointmentDate = DateTime.Now;  float _Pa_ID_Fees = 0;  
            int _CreatedByUser_ID = -1; bool IsLocked=false;int RetakeTest_Application_ID = -1;

            if (clsTestAppointmentData.GetTestAppointmentInfoBy_ID(TestAppointment_ID, ref  TestType_ID, ref  _LocalDrivingLicense_Application_ID,
            ref   AppointmentDate, ref  _Pa_ID_Fees, ref  _CreatedByUser_ID, ref IsLocked, ref RetakeTest_Application_ID))

                return new clsTestAppointments(TestAppointment_ID,  (clsTestTypes.enTestType) TestType_ID,  _LocalDrivingLicense_Application_ID,
             AppointmentDate,  _Pa_ID_Fees,  _CreatedByUser_ID, IsLocked, RetakeTest_Application_ID);
            else
                return null;

        }

        public static clsTestAppointments GetLastTestAppointment(int _LocalDrivingLicense_Application_ID, clsTestTypes.enTestType TestType_ID )
        {
             int TestAppointment_ID=-1;
            DateTime AppointmentDate = DateTime.Now; float _Pa_ID_Fees = 0;
            int _CreatedByUser_ID = -1;bool IsLocked=false;int RetakeTest_Application_ID=-1;

            if (clsTestAppointmentData.GetLastTestAppointment( _LocalDrivingLicense_Application_ID, (int) TestType_ID,
                ref TestAppointment_ID,ref AppointmentDate, ref _Pa_ID_Fees, ref _CreatedByUser_ID,ref IsLocked,ref RetakeTest_Application_ID))

                return new clsTestAppointments(TestAppointment_ID, TestType_ID, _LocalDrivingLicense_Application_ID,
             AppointmentDate, _Pa_ID_Fees, _CreatedByUser_ID, IsLocked, RetakeTest_Application_ID);
            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();

        }

        public DataTable GetAll(int TestType_ID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this._LocalDrivingLicenseApplicationID,TestType_ID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int _LocalDrivingLicense_Application_ID,clsTestTypes.enTestType TestType_ID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicense_Application_ID, (int) TestType_ID);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }

        private int  _GetTest_ID()
        {
            return clsTestAppointmentData.GetTest_ID(TestAppointment_ID);
        }
        public static int GetTrail(int testTyp_id,int LocalLicense_id)
        {
            return clsTestAppointmentData.GetTrail(testTyp_id,LocalLicense_id);
        }

    }
}
